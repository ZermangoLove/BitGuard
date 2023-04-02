using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000F5 RID: 245
	internal class MethodPatcher
	{
		// Token: 0x060003C8 RID: 968 RVA: 0x00013DF4 File Offset: 0x00011FF4
		public static string GeneratenamiSpaci()
		{
			return new string((from s in Enumerable.Repeat<string>("123234234234349234972734982378647326847723684768327684678", 10)
				select s[MethodPatcher.rdn.Next(s.Length)]).ToArray<char>());
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00013E40 File Offset: 0x00012040
		public static string GenerateString()
		{
			return new string((from s in Enumerable.Repeat<string>("123234234234349234972734982378647326847723684768327684678", 10)
				select s[MethodPatcher.rdn.Next(s.Length)]).ToArray<char>());
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00013E8C File Offset: 0x0001208C
		public MethodPatcher(ModuleDef rtModule)
		{
			TypeDef entry2 = rtModule.Find(RTMap.VMEntry, true);
			UTF8String Name = (entry2.Name = MethodPatcher.GenerateString());
			UTF8String utf8String = (entry2.Namespace = "");
			UTF8String All = Name;
			foreach (MethodDef entry3 in rtModule.Find(All, true).FindMethods(RTMap.VMRun))
			{
				foreach (MethodDef _entry in rtModule.Find(All, true).FindMethods(RTMap.VMRun2))
				{
					_entry.Name = MethodPatcher.GenerateString();
				}
				bool flag = entry3.Parameters.Count == 2;
				if (flag)
				{
					entry3.Name = All;
					this.vmEntryNormal = entry3;
					this.vmEntryNormal.Name = All;
				}
				else
				{
					entry3.Name = MethodPatcher.GenerateString();
					this.vmEntryTyped = entry3;
					this.vmEntryTyped.Name = All;
				}
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014004 File Offset: 0x00012204
		private static bool ShouldBeTyped(MethodDef method)
		{
			bool flag = !method.IsStatic && method.DeclaringType.IsValueType;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				foreach (Parameter param in ((IEnumerable<Parameter>)method.Parameters))
				{
					bool isByRef = param.Type.IsByRef;
					if (isByRef)
					{
						return true;
					}
				}
				bool isByRef2 = method.ReturnType.IsByRef;
				flag2 = isByRef2;
			}
			return flag2;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001409C File Offset: 0x0001229C
		public void PatchMethodStub(MethodDef method, uint id)
		{
			bool flag = MethodPatcher.ShouldBeTyped(method);
			if (flag)
			{
				this.PatchTyped(method.Module, method, (int)id);
			}
			else
			{
				this.PatchNormal(method.Module, method, (int)id);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000140D4 File Offset: 0x000122D4
		private void PatchNormal(ModuleDef module, MethodDef method, int id)
		{
			CilBody body = new CilBody();
			method.Body = body;
			body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, id));
			body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
			body.Instructions.Add(Instruction.Create(OpCodes.Newarr, method.Module.CorLibTypes.Object.ToTypeDefOrRef()));
			foreach (Parameter param in ((IEnumerable<Parameter>)method.Parameters))
			{
				body.Instructions.Add(Instruction.Create(OpCodes.Dup));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, param.Index));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, param));
				bool isValueType = param.Type.IsValueType;
				if (isValueType)
				{
					body.Instructions.Add(Instruction.Create(OpCodes.Box, param.Type.ToTypeDefOrRef()));
				}
				else
				{
					bool isPointer = param.Type.IsPointer;
					if (isPointer)
					{
						body.Instructions.Add(Instruction.Create(OpCodes.Conv_U));
						body.Instructions.Add(Instruction.Create(OpCodes.Box, method.Module.CorLibTypes.UIntPtr.ToTypeDefOrRef()));
					}
				}
				body.Instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
			}
			body.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(this.vmEntryNormal)));
			bool flag = method.ReturnType.ElementType == ElementType.Void;
			if (flag)
			{
				body.Instructions.Add(Instruction.Create(OpCodes.Pop));
			}
			else
			{
				bool isValueType2 = method.ReturnType.IsValueType;
				if (isValueType2)
				{
					body.Instructions.Add(Instruction.Create(OpCodes.Unbox_Any, method.ReturnType.ToTypeDefOrRef()));
				}
				else
				{
					body.Instructions.Add(Instruction.Create(OpCodes.Castclass, method.ReturnType.ToTypeDefOrRef()));
				}
			}
			body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			body.OptimizeMacros();
			MutateRT.IntControlFlow(method);
			MutateRT.Array_Mutation(method);
			MutateRT.BasicEncodeInt(method);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014364 File Offset: 0x00012564
		private void PatchTyped(ModuleDef module, MethodDef method, int id)
		{
			CilBody body = new CilBody();
			method.Body = body;
			body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, id));
			body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
			body.Instructions.Add(Instruction.Create(OpCodes.Newarr, new PtrSig(method.Module.CorLibTypes.Void).ToTypeDefOrRef()));
			foreach (Parameter param in ((IEnumerable<Parameter>)method.Parameters))
			{
				body.Instructions.Add(Instruction.Create(OpCodes.Dup));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, param.Index));
				bool isByRef = param.Type.IsByRef;
				if (isByRef)
				{
					body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, param));
					body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, param.Type.Next.ToTypeDefOrRef()));
				}
				else
				{
					body.Instructions.Add(Instruction.Create(OpCodes.Ldarga, param));
					body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, param.Type.ToTypeDefOrRef()));
				}
				Local local = new Local(method.Module.CorLibTypes.TypedReference);
				body.Variables.Add(local);
				body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, local));
				body.Instructions.Add(Instruction.Create(OpCodes.Conv_I));
				body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I));
			}
			bool flag = method.ReturnType.GetElementType() != ElementType.Void;
			if (flag)
			{
				Local retVar = new Local(method.ReturnType);
				Local retRef = new Local(method.Module.CorLibTypes.TypedReference);
				body.Variables.Add(retVar);
				body.Variables.Add(retRef);
				body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, retVar));
				body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, method.ReturnType.ToTypeDefOrRef()));
				body.Instructions.Add(Instruction.Create(OpCodes.Stloc, retRef));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, retRef));
				body.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(this.vmEntryTyped)));
				body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, retVar));
			}
			else
			{
				body.Instructions.Add(Instruction.Create(OpCodes.Ldnull));
				body.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(this.vmEntryTyped)));
			}
			body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			body.OptimizeMacros();
			MutateRT.IntControlFlow(method);
			MutateRT.Array_Mutation(method);
			MutateRT.BasicEncodeInt(method);
		}

		// Token: 0x04000190 RID: 400
		private MethodDef vmEntryNormal;

		// Token: 0x04000191 RID: 401
		private MethodDef vmEntryTyped;

		// Token: 0x04000192 RID: 402
		public static Random rdn = new Random();
	}
}
