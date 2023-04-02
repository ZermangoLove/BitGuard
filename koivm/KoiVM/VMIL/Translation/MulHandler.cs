using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C1 RID: 193
	public class MulHandler : ITranslationHandler
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00010B50 File Offset: 0x0000ED50
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.MUL;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010B64 File Offset: 0x0000ED64
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			switch (TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type))
			{
			case ASTType.I4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.MUL_DWORD));
				goto IL_B9;
			case ASTType.I8:
			case ASTType.Ptr:
			case ASTType.ByRef:
				tr.Instructions.Add(new ILInstruction(ILOpCode.MUL_QWORD));
				goto IL_B9;
			case ASTType.R4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.MUL_R32));
				goto IL_B9;
			case ASTType.R8:
				tr.Instructions.Add(new ILInstruction(ILOpCode.MUL_R64));
				goto IL_B9;
			}
			throw new NotSupportedException();
			IL_B9:
			tr.PopOperand(instr.Operand1);
		}
	}
}
