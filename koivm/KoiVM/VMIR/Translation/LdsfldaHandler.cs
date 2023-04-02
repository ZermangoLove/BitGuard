using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000067 RID: 103
	public class LdsfldaHandler : ITranslationHandler
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000B400 File Offset: 0x00009600
		public Code ILCode
		{
			get
			{
				return Code.Ldsflda;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B414 File Offset: 0x00009614
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int fieldId = (int)(tr.VM.Data.GetId((IField)expr.Operand) | 2147483648U);
			int ecallId = tr.VM.Runtime.VMCall.LDFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.Null()));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
