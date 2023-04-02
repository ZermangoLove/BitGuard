using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D1 RID: 209
	public class NopHandler : ITranslationHandler
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00011410 File Offset: 0x0000F610
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.NOP;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00002BCF File Offset: 0x00000DCF
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.NOP));
		}
	}
}
