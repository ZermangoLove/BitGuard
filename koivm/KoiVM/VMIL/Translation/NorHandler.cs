using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000BC RID: 188
	public class NorHandler : ITranslationHandler
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000107BC File Offset: 0x0000E9BC
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.NOR;
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000107D0 File Offset: 0x0000E9D0
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			ASTType type = TypeInference.InferIntegerOp(instr.Operand1.Type, instr.Operand2.Type);
			ASTType asttype = type;
			ASTType asttype2 = asttype;
			if (asttype2 != ASTType.I4)
			{
				if (asttype2 != ASTType.I8 && asttype2 != ASTType.Ptr)
				{
					throw new NotSupportedException();
				}
				tr.Instructions.Add(new ILInstruction(ILOpCode.NOR_QWORD));
			}
			else
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.NOR_DWORD));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
