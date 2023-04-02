using System;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D3 RID: 211
	public class PopHandler : ITranslationHandler
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00011438 File Offset: 0x0000F638
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.POP;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00002BF4 File Offset: 0x00000DF4
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PopOperand(instr.Operand1);
		}
	}
}
