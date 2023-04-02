using System;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D4 RID: 212
	public class MovHandler : ITranslationHandler
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0001144C File Offset: 0x0000F64C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.MOV;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00002C04 File Offset: 0x00000E04
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			tr.PopOperand(instr.Operand1);
		}
	}
}
