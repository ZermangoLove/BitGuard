using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM
{
	// Token: 0x02000003 RID: 3
	public class GenericInstantiation
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000007 RID: 7 RVA: 0x00003F68 File Offset: 0x00002168
		// (remove) Token: 0x06000008 RID: 8 RVA: 0x00003FA0 File Offset: 0x000021A0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Func<MethodSpec, bool> ShouldInstantiate;

		// Token: 0x06000009 RID: 9 RVA: 0x00003FD8 File Offset: 0x000021D8
		public void EnsureInstantiation(MethodDef method, Action<MethodSpec, MethodDef> onInstantiated)
		{
			foreach (Instruction instr in method.Body.Instructions)
			{
				bool flag = instr.Operand is MethodSpec;
				if (flag)
				{
					MethodSpec spec = (MethodSpec)instr.Operand;
					bool flag2 = this.ShouldInstantiate != null && !this.ShouldInstantiate(spec);
					if (!flag2)
					{
						MethodDef instantiation;
						bool flag3 = !this.Instantiate(spec, out instantiation);
						if (flag3)
						{
							onInstantiated(spec, instantiation);
						}
						instr.Operand = instantiation;
					}
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00004090 File Offset: 0x00002290
		public bool Instantiate(MethodSpec methodSpec, out MethodDef def)
		{
			bool flag = this.instantiations.TryGetValue(methodSpec, out def);
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				GenericArguments genericArguments = new GenericArguments();
				genericArguments.PushMethodArgs(methodSpec.GenericInstMethodSig.GenericArguments);
				MethodDef originDef = methodSpec.Method.ResolveMethodDefThrow();
				MethodSig newSig = this.ResolveMethod(originDef.MethodSig, genericArguments);
				newSig.Generic = false;
				newSig.GenParamCount = 0U;
				string newName = originDef.Name;
				foreach (TypeSig typeArg in methodSpec.GenericInstMethodSig.GenericArguments)
				{
					newName = newName + ";" + typeArg.TypeName;
				}
				def = new MethodDefUser(newName, newSig, originDef.ImplAttributes, originDef.Attributes);
				TypeSig thisParam = (originDef.HasThis ? originDef.Parameters[0].Type : null);
				def.DeclaringType2 = originDef.DeclaringType2;
				bool flag3 = thisParam != null;
				if (flag3)
				{
					def.Parameters[0].Type = thisParam;
				}
				foreach (DeclSecurity declSec in originDef.DeclSecurities)
				{
					def.DeclSecurities.Add(declSec);
				}
				def.ImplMap = originDef.ImplMap;
				foreach (MethodOverride ov in originDef.Overrides)
				{
					def.Overrides.Add(ov);
				}
				def.Body = new CilBody();
				def.Body.InitLocals = originDef.Body.InitLocals;
				def.Body.MaxStack = originDef.Body.MaxStack;
				foreach (Local variable in originDef.Body.Variables)
				{
					Local newVar = new Local(variable.Type);
					def.Body.Variables.Add(newVar);
				}
				Dictionary<Instruction, Instruction> instrMap = new Dictionary<Instruction, Instruction>();
				foreach (Instruction instr in originDef.Body.Instructions)
				{
					Instruction newInstr = new Instruction(instr.OpCode, this.ResolveOperand(instr.Operand, genericArguments));
					def.Body.Instructions.Add(newInstr);
					instrMap[instr] = newInstr;
				}
				foreach (Instruction instr2 in def.Body.Instructions)
				{
					bool flag4 = instr2.Operand is Instruction;
					if (flag4)
					{
						instr2.Operand = instrMap[(Instruction)instr2.Operand];
					}
					else
					{
						bool flag5 = instr2.Operand is Instruction[];
						if (flag5)
						{
							Instruction[] targets = (Instruction[])((Instruction[])instr2.Operand).Clone();
							for (int i = 0; i < targets.Length; i++)
							{
								targets[i] = instrMap[targets[i]];
							}
							instr2.Operand = targets;
						}
					}
				}
				def.Body.UpdateInstructionOffsets();
				foreach (ExceptionHandler eh in originDef.Body.ExceptionHandlers)
				{
					ExceptionHandler newEH = new ExceptionHandler(eh.HandlerType);
					newEH.TryStart = instrMap[eh.TryStart];
					newEH.HandlerStart = instrMap[eh.HandlerStart];
					bool flag6 = eh.TryEnd != null;
					if (flag6)
					{
						newEH.TryEnd = instrMap[eh.TryEnd];
					}
					bool flag7 = eh.HandlerEnd != null;
					if (flag7)
					{
						newEH.HandlerEnd = instrMap[eh.HandlerEnd];
					}
					bool flag8 = eh.CatchType != null;
					if (flag8)
					{
						newEH.CatchType = genericArguments.Resolve(newEH.CatchType.ToTypeSig(true)).ToTypeDefOrRef();
					}
					else
					{
						bool flag9 = eh.FilterStart != null;
						if (flag9)
						{
							newEH.FilterStart = instrMap[eh.FilterStart];
						}
					}
					def.Body.ExceptionHandlers.Add(newEH);
				}
				this.instantiations[methodSpec] = def;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00004624 File Offset: 0x00002824
		private FieldSig ResolveField(FieldSig sig, GenericArguments genericArgs)
		{
			FieldSig newSig = sig.Clone();
			newSig.Type = genericArgs.ResolveType(newSig.Type);
			return newSig;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00004654 File Offset: 0x00002854
		private GenericInstMethodSig ResolveInst(GenericInstMethodSig sig, GenericArguments genericArgs)
		{
			GenericInstMethodSig newSig = sig.Clone();
			for (int i = 0; i < newSig.GenericArguments.Count; i++)
			{
				newSig.GenericArguments[i] = genericArgs.ResolveType(newSig.GenericArguments[i]);
			}
			return newSig;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000046A8 File Offset: 0x000028A8
		private MethodSig ResolveMethod(MethodSig sig, GenericArguments genericArgs)
		{
			MethodSig newSig = sig.Clone();
			for (int i = 0; i < newSig.Params.Count; i++)
			{
				newSig.Params[i] = genericArgs.ResolveType(newSig.Params[i]);
			}
			bool flag = newSig.ParamsAfterSentinel != null;
			if (flag)
			{
				for (int j = 0; j < newSig.ParamsAfterSentinel.Count; j++)
				{
					newSig.ParamsAfterSentinel[j] = genericArgs.ResolveType(newSig.ParamsAfterSentinel[j]);
				}
			}
			newSig.RetType = genericArgs.ResolveType(newSig.RetType);
			return newSig;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00004760 File Offset: 0x00002960
		private object ResolveOperand(object operand, GenericArguments genericArgs)
		{
			bool flag = operand is MemberRef;
			object obj;
			if (flag)
			{
				MemberRef memberRef = (MemberRef)operand;
				bool isFieldRef = memberRef.IsFieldRef;
				if (isFieldRef)
				{
					FieldSig field = this.ResolveField(memberRef.FieldSig, genericArgs);
					memberRef = new MemberRefUser(memberRef.Module, memberRef.Name, field, memberRef.Class);
				}
				else
				{
					MethodSig method = this.ResolveMethod(memberRef.MethodSig, genericArgs);
					memberRef = new MemberRefUser(memberRef.Module, memberRef.Name, method, memberRef.Class);
				}
				obj = memberRef;
			}
			else
			{
				bool flag2 = operand is TypeSpec;
				if (flag2)
				{
					TypeSig sig = ((TypeSpec)operand).TypeSig;
					obj = genericArgs.ResolveType(sig).ToTypeDefOrRef();
				}
				else
				{
					bool flag3 = operand is MethodSpec;
					if (flag3)
					{
						MethodSpec spec = (MethodSpec)operand;
						spec = new MethodSpecUser(spec.Method, this.ResolveInst(spec.GenericInstMethodSig, genericArgs));
						obj = spec;
					}
					else
					{
						obj = operand;
					}
				}
			}
			return obj;
		}

		// Token: 0x04000003 RID: 3
		private readonly Dictionary<MethodSpec, MethodDef> instantiations = new Dictionary<MethodSpec, MethodDef>(MethodEqualityComparer.CompareDeclaringTypes);
	}
}
