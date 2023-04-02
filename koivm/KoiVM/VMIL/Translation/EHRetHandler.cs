using System;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000D9 RID: 217
	public class EHRetHandler : ITranslationHandler
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__EHRET;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000114C4 File Offset: 0x0000F6C4
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			bool flag = instr.Operand1 != null;
			if (flag)
			{
				tr.PushOperand(instr.Operand1);
				tr.Instructions.Add(new ILInstruction(ILOpCode.POP, ILRegister.R0));
			}
			tr.Instructions.Add(new ILInstruction(ILOpCode.RET));
		}
	}
}
