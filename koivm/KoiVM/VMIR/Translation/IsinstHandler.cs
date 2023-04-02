using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000045 RID: 69
	public class IsinstHandler : ITranslationHandler
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00008A90 File Offset: 0x00006C90
		public Code ILCode
		{
			get
			{
				return Code.Isinst;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008AA4 File Offset: 0x00006CA4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int typeId = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.CAST;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
