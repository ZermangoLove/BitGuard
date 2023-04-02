using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000088 RID: 136
	public class BrHandler : ITranslationHandler
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		public Code ILCode
		{
			get
			{
				return Code.Br;
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.JMP)
			{
				Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand)
			});
			return null;
		}
	}
}
