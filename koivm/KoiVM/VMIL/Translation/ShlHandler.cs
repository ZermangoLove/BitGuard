using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000BD RID: 189
	public class ShlHandler : ITranslationHandler
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00010868 File Offset: 0x0000EA68
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.SHL;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001087C File Offset: 0x0000EA7C
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
				tr.Instructions.Add(new ILInstruction(ILOpCode.SHL_QWORD));
			}
			else
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.SHL_DWORD));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
