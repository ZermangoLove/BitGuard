using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C8 RID: 200
	public class JmpHandler : ITranslationHandler
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00011200 File Offset: 0x0000F400
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.JMP;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00002AB2 File Offset: 0x00000CB2
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.JMP)
			{
				Annotation = InstrAnnotation.JUMP
			});
		}
	}
}
