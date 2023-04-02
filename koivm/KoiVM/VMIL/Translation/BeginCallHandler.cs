using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D7 RID: 215
	public class BeginCallHandler : ITranslationHandler
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00011488 File Offset: 0x0000F688
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__BEGINCALL;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00002C4D File Offset: 0x00000E4D
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.__BEGINCALL)
			{
				Annotation = instr.Annotation
			});
		}
	}
}
