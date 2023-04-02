using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CE RID: 206
	public class CallHandler : ITranslationHandler
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0001138C File Offset: 0x0000F58C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.CALL;
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00002B89 File Offset: 0x00000D89
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.CALL)
			{
				Annotation = instr.Annotation
			});
		}
	}
}
