using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C0 RID: 192
	public class SubHandler : ITranslationHandler
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00010AA8 File Offset: 0x0000ECA8
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.SUB;
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00010ABC File Offset: 0x0000ECBC
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			ASTType type = TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type);
			ASTType asttype = type;
			ASTType asttype2 = asttype;
			if (asttype2 != ASTType.R4)
			{
				if (asttype2 != ASTType.R8)
				{
					throw new NotSupportedException();
				}
				tr.Instructions.Add(new ILInstruction(ILOpCode.SUB_R64));
			}
			else
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.SUB_R32));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
