using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CD RID: 205
	public class LeaveHandler : ITranslationHandler
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00011378 File Offset: 0x0000F578
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.LEAVE;
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00002B59 File Offset: 0x00000D59
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.LEAVE)
			{
				Annotation = instr.Annotation
			});
		}
	}
}
