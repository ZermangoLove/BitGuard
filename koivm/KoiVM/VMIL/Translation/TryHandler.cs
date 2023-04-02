using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CC RID: 204
	public class TryHandler : ITranslationHandler
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0001130C File Offset: 0x0000F50C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.TRY;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011320 File Offset: 0x0000F520
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			bool flag = instr.Operand2 != null;
			if (flag)
			{
				tr.PushOperand(instr.Operand2);
			}
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.TRY)
			{
				Annotation = instr.Annotation
			});
		}
	}
}
