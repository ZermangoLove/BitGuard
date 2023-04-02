using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D0 RID: 208
	public class VcallHandler : ITranslationHandler
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000324 RID: 804 RVA: 0x000113B4 File Offset: 0x0000F5B4
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.VCALL;
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000113C8 File Offset: 0x0000F5C8
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			bool flag = instr.Operand2 != null;
			if (flag)
			{
				tr.PushOperand(instr.Operand2);
			}
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.VCALL));
		}
	}
}
