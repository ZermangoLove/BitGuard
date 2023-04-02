using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000BE RID: 190
	public class ShrHandler : ITranslationHandler
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00010914 File Offset: 0x0000EB14
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.SHR;
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00010928 File Offset: 0x0000EB28
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			ASTType type = TypeInference.InferShiftOp(instr.Operand1.Type, instr.Operand2.Type);
			ASTType asttype = type;
			ASTType asttype2 = asttype;
			if (asttype2 != ASTType.I4)
			{
				if (asttype2 != ASTType.I8 && asttype2 != ASTType.Ptr)
				{
					throw new NotSupportedException();
				}
				tr.Instructions.Add(new ILInstruction(ILOpCode.SHR_QWORD));
			}
			else
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.SHR_DWORD));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
