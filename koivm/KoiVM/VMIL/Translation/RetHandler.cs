using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CF RID: 207
	public class RetHandler : ITranslationHandler
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000113A0 File Offset: 0x0000F5A0
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.RET;
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00002BB9 File Offset: 0x00000DB9
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.RET));
		}
	}
}
