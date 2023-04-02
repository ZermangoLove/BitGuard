using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D5 RID: 213
	public class EntryHandler : ITranslationHandler
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00011460 File Offset: 0x0000F660
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__ENTRY;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00002C21 File Offset: 0x00000E21
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.__ENTRY));
		}
	}
}
