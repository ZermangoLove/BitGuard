using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.VM;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008F RID: 143
	public class RetHandler : ITranslationHandler
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		public Code ILCode
		{
			get
			{
				return Code.Ret;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			bool flag = expr.Arguments.Length == 1;
			if (flag)
			{
				IIROperand value = tr.Translate(expr.Arguments[0]);
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = new IRRegister(VMRegisters.R0, value.Type),
					Operand2 = value
				});
			}
			else
			{
				Debug.Assert(expr.Arguments.Length == 0);
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.RET));
			return null;
		}
	}
}
