using System;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C7 RID: 199
	public class CmpHandler : ITranslationHandler
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000110AC File Offset: 0x0000F2AC
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.CMP;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000110C0 File Offset: 0x0000F2C0
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			tr.PushOperand(instr.Operand2);
			bool flag = instr.Operand1.Type == ASTType.O || instr.Operand2.Type == ASTType.O;
			if (flag)
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.CMP));
			}
			else
			{
				bool flag2 = instr.Operand1.Type == ASTType.I8 || instr.Operand2.Type == ASTType.I8 || instr.Operand1.Type == ASTType.Ptr || instr.Operand2.Type == ASTType.Ptr;
				if (flag2)
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.CMP_QWORD));
				}
				else
				{
					bool flag3 = instr.Operand1.Type == ASTType.R8 || instr.Operand2.Type == ASTType.R8;
					if (flag3)
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.CMP_R64));
					}
					else
					{
						bool flag4 = instr.Operand1.Type == ASTType.R4 || instr.Operand2.Type == ASTType.R4;
						if (flag4)
						{
							tr.Instructions.Add(new ILInstruction(ILOpCode.CMP_R32));
						}
						else
						{
							tr.Instructions.Add(new ILInstruction(ILOpCode.CMP_DWORD));
						}
					}
				}
			}
		}
	}
}
