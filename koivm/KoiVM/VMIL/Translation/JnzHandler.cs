using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CA RID: 202
	public class JnzHandler : ITranslationHandler
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00011228 File Offset: 0x0000F428
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.JNZ;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00002B1D File Offset: 0x00000D1D
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.JNZ)
			{
				Annotation = InstrAnnotation.JUMP
			});
		}
	}
}
