using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C2 RID: 194
	public class DivHandler : ITranslationHandler
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00010C38 File Offset: 0x0000EE38
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.DIV;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010C4C File Offset: 0x0000EE4C
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			switch (TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type))
			{
			case ASTType.I4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.DIV_DWORD));
				goto IL_B9;
			case ASTType.I8:
			case ASTType.Ptr:
			case ASTType.ByRef:
				tr.Instructions.Add(new ILInstruction(ILOpCode.DIV_QWORD));
				goto IL_B9;
			case ASTType.R4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.DIV_R32));
				goto IL_B9;
			case ASTType.R8:
				tr.Instructions.Add(new ILInstruction(ILOpCode.DIV_R64));
				goto IL_B9;
			}
			throw new NotSupportedException();
			IL_B9:
			tr.PopOperand(instr.Operand1);
		}
	}
}
