using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000065 RID: 101
	public class StsfldHandler : ITranslationHandler
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000B254 File Offset: 0x00009454
		public Code ILCode
		{
			get
			{
				return Code.Stsfld;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B26C File Offset: 0x0000946C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand val = tr.Translate(expr.Arguments[0]);
			int fieldId = (int)tr.VM.Data.GetId((IField)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.STFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.Null()));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, val));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			return null;
		}
	}
}
