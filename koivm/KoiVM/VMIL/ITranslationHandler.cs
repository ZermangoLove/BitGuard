using System;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL
{
	// Token: 0x020000BA RID: 186
	public interface ITranslationHandler
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002DA RID: 730
		IROpCode IRCode { get; }

		// Token: 0x060002DB RID: 731
		void Translate(IRInstruction instr, ILTranslator tr);
	}
}
