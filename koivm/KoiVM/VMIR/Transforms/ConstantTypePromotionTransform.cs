using System;
using System.Diagnostics;
using KoiVM.AST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A2 RID: 162
	public class ConstantTypePromotionTransform : ITransform
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00002755 File Offset: 0x00000955
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000E50C File Offset: 0x0000C70C
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			IROpCode opCode = instr.OpCode;
			IROpCode iropCode = opCode;
			if (iropCode != IROpCode.MOV)
			{
				switch (iropCode)
				{
				case IROpCode.NOR:
				case IROpCode.CMP:
				case IROpCode.ADD:
				case IROpCode.MUL:
				case IROpCode.DIV:
				case IROpCode.REM:
					goto IL_4F;
				case IROpCode.JZ:
				case IROpCode.JNZ:
				case IROpCode.JMP:
				case IROpCode.SWT:
				case IROpCode.SUB:
					break;
				default:
					if (iropCode - IROpCode.__AND <= 3)
					{
						goto IL_4F;
					}
					break;
				}
				return;
			}
			IL_4F:
			Debug.Assert(instr.Operand1 != null && instr.Operand2 != null);
			bool flag = instr.Operand1 is IRConstant;
			if (flag)
			{
				instr.Operand1 = ConstantTypePromotionTransform.PromoteConstant((IRConstant)instr.Operand1, instr.Operand2.Type);
			}
			bool flag2 = instr.Operand2 is IRConstant;
			if (flag2)
			{
				instr.Operand2 = ConstantTypePromotionTransform.PromoteConstant((IRConstant)instr.Operand2, instr.Operand1.Type);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		private static IIROperand PromoteConstant(IRConstant value, ASTType type)
		{
			IIROperand iiroperand;
			switch (type)
			{
			case ASTType.I8:
				iiroperand = ConstantTypePromotionTransform.PromoteConstantI8(value);
				break;
			case ASTType.R4:
				iiroperand = ConstantTypePromotionTransform.PromoteConstantR4(value);
				break;
			case ASTType.R8:
				iiroperand = ConstantTypePromotionTransform.PromoteConstantR8(value);
				break;
			default:
				iiroperand = value;
				break;
			}
			return iiroperand;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000E640 File Offset: 0x0000C840
		private static IIROperand PromoteConstantI8(IRConstant value)
		{
			bool flag = value.Type.Value == ASTType.I4;
			if (flag)
			{
				value.Type = new ASTType?(ASTType.I8);
				value.Value = (long)((int)value.Value);
			}
			else
			{
				bool flag2 = value.Type.Value != ASTType.I8;
				if (flag2)
				{
					throw new InvalidProgramException();
				}
			}
			return value;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000E6B0 File Offset: 0x0000C8B0
		private static IIROperand PromoteConstantR4(IRConstant value)
		{
			bool flag = value.Type.Value == ASTType.I4;
			if (flag)
			{
				value.Type = new ASTType?(ASTType.R4);
				value.Value = (float)((int)value.Value);
			}
			else
			{
				bool flag2 = value.Type.Value != ASTType.R4;
				if (flag2)
				{
					throw new InvalidProgramException();
				}
			}
			return value;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000E720 File Offset: 0x0000C920
		private static IIROperand PromoteConstantR8(IRConstant value)
		{
			bool flag = value.Type.Value == ASTType.I4;
			if (flag)
			{
				value.Type = new ASTType?(ASTType.R8);
				value.Value = (double)((int)value.Value);
			}
			else
			{
				bool flag2 = value.Type.Value == ASTType.I8;
				if (flag2)
				{
					value.Type = new ASTType?(ASTType.R8);
					value.Value = (double)((long)value.Value);
				}
				else
				{
					bool flag3 = value.Type.Value == ASTType.R4;
					if (flag3)
					{
						value.Type = new ASTType?(ASTType.R8);
						value.Value = (double)((float)value.Value);
					}
					else
					{
						bool flag4 = value.Type.Value != ASTType.R8;
						if (flag4)
						{
							throw new InvalidProgramException();
						}
					}
				}
			}
			return value;
		}
	}
}
