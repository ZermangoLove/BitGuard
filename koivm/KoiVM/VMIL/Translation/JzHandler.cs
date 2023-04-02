using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C9 RID: 201
	public class JzHandler : ITranslationHandler
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00011214 File Offset: 0x0000F414
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.JZ;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00002AE1 File Offset: 0x00000CE1
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			tr.PushOperand(instr.Operand1);
			tr.Instructions.Add(new ILInstruction(ILOpCode.JZ)
			{
				Annotation = InstrAnnotation.JUMP
			});
		}
	}
}
