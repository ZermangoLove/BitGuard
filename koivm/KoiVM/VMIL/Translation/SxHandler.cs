using System;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C4 RID: 196
	public class SxHandler : ITranslationHandler
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00010E04 File Offset: 0x0000F004
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.SX;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010E18 File Offset: 0x0000F018
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			ASTType type = instr.Operand1.Type;
			ASTType asttype = type;
			if (asttype != ASTType.I4)
			{
				if (asttype != ASTType.I8)
				{
					throw new NotSupportedException();
				}
				tr.Instructions.Add(new ILInstruction(ILOpCode.SX_DWORD));
			}
			else
			{
				bool flag = instr.Operand1 is IRVariable;
				if (flag)
				{
					ElementType rawType = ((IRVariable)instr.Operand1).RawType.ElementType;
					bool flag2 = rawType == ElementType.I2;
					if (flag2)
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.SX_WORD));
					}
				}
				tr.Instructions.Add(new ILInstruction(ILOpCode.SX_BYTE));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
