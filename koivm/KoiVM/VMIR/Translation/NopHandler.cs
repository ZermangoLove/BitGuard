using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006C RID: 108
	public class NopHandler : ITranslationHandler
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000B6F4 File Offset: 0x000098F4
		public Code ILCode
		{
			get
			{
				return Code.Nop;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B708 File Offset: 0x00009908
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.NOP));
			return null;
		}
	}
}
