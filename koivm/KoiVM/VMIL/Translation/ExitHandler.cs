using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D6 RID: 214
	public class ExitHandler : ITranslationHandler
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00011474 File Offset: 0x0000F674
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__EXIT;
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00002C37 File Offset: 0x00000E37
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.Instructions.Add(new ILInstruction(ILOpCode.__EXIT));
		}
	}
}
