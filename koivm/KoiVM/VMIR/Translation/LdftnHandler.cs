using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000060 RID: 96
	public class LdftnHandler : ITranslationHandler
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000AD48 File Offset: 0x00008F48
		public Code ILCode
		{
			get
			{
				return Code.Ldftn;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AD60 File Offset: 0x00008F60
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			MethodDef method = ((IMethod)expr.Operand).ResolveMethodDef();
			bool intraLinking = method != null && tr.VM.Settings.IsVirtualized(method);
			int ecallId = tr.VM.Runtime.VMCall.LDFTN;
			bool flag = intraLinking;
			if (flag)
			{
				int sigId = (int)tr.VM.Data.GetId(method.DeclaringType, method.MethodSig);
				uint entryKey = (uint)tr.VM.Data.LookupInfo(method).EntryKey;
				entryKey = (uint)((tr.VM.Random.Next() & -256) | (int)entryKey);
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI4((int)entryKey)));
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI4(sigId)));
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, new IRMetaTarget(method)
				{
					LateResolve = true
				}));
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId)));
			}
			else
			{
				int methodId = (int)tr.VM.Data.GetId((IMethod)expr.Operand);
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI4(0)));
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(methodId)));
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
