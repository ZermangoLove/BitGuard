using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000063 RID: 99
	public class StfldHandler : ITranslationHandler
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000B0C4 File Offset: 0x000092C4
		public Code ILCode
		{
			get
			{
				return Code.Stfld;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B0D8 File Offset: 0x000092D8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IIROperand obj = tr.Translate(expr.Arguments[0]);
			IIROperand val = tr.Translate(expr.Arguments[1]);
			int fieldId = (int)tr.VM.Data.GetId((IField)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.STFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, obj));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, val));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			return null;
		}
	}
}
