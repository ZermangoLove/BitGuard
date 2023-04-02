using System;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D2 RID: 210
	public class PushHandler : ITranslationHandler
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00011424 File Offset: 0x0000F624
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.PUSH;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
		}
	}
}
