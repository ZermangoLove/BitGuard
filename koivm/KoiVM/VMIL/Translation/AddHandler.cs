using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000BF RID: 191
	public class AddHandler : ITranslationHandler
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000109C0 File Offset: 0x0000EBC0
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.ADD;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000109D4 File Offset: 0x0000EBD4
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			switch (TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type))
			{
			case ASTType.I4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_DWORD));
				goto IL_B9;
			case ASTType.I8:
			case ASTType.Ptr:
			case ASTType.ByRef:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_QWORD));
				goto IL_B9;
			case ASTType.R4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_R32));
				goto IL_B9;
			case ASTType.R8:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_R64));
				goto IL_B9;
			}
			throw new NotSupportedException();
			IL_B9:
			tr.PopOperand(instr.Operand1);
		}
	}
}
