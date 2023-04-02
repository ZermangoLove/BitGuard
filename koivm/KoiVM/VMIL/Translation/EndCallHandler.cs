using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D8 RID: 216
	public class EndCallHandler : ITranslationHandler
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0001149C File Offset: 0x0000F69C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__ENDCALL;
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00002C70 File Offset: 0x00000E70
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.__ENDCALL)
			{
				Annotation = instr.Annotation
			});
		}
	}
}
