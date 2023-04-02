using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C3 RID: 195
	public class RemHandler : ITranslationHandler
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00010D20 File Offset: 0x0000EF20
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.REM;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00010D34 File Offset: 0x0000EF34
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			switch (TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type))
			{
			case ASTType.I4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.REM_DWORD));
				goto IL_B5;
			case ASTType.I8:
			case ASTType.Ptr:
				tr.Instructions.Add(new ILInstruction(ILOpCode.REM_QWORD));
				goto IL_B5;
			case ASTType.R4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.REM_R32));
				goto IL_B5;
			case ASTType.R8:
				tr.Instructions.Add(new ILInstruction(ILOpCode.REM_R64));
				goto IL_B5;
			}
			throw new NotSupportedException();
			IL_B5:
			tr.PopOperand(instr.Operand1);
		}
	}
}
