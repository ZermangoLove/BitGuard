using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008B RID: 139
	public class SwitchHandler : ITranslationHandler
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		public Code ILCode
		{
			get
			{
				return Code.Switch;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand val = tr.Translate(expr.Arguments[0]);
			tr.Instructions.Add(new IRInstruction(IROpCode.SWT)
			{
				Operand1 = new IRJumpTable((IBasicBlock[])expr.Operand),
				Operand2 = val
			});
			return null;
		}
	}
}
