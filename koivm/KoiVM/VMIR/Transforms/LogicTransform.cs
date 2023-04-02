using System;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000AF RID: 175
	public class LogicTransform : ITransform
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000028E1 File Offset: 0x00000AE1
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000F908 File Offset: 0x0000DB08
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.__NOT;
			if (flag)
			{
				instrs.Replace(index, new IRInstruction[]
				{
					new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr)
				});
			}
			else
			{
				bool flag2 = instr.OpCode == IROpCode.__AND;
				if (flag2)
				{
					IRVariable tmp = tr.Context.AllocateVRegister(instr.Operand2.Type);
					instrs.Replace(index, new IRInstruction[]
					{
						new IRInstruction(IROpCode.MOV, tmp, instr.Operand2, instr),
						new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr),
						new IRInstruction(IROpCode.NOR, tmp, tmp, instr),
						new IRInstruction(IROpCode.NOR, instr.Operand1, tmp, instr)
					});
				}
				else
				{
					bool flag3 = instr.OpCode == IROpCode.__OR;
					if (flag3)
					{
						instrs.Replace(index, new IRInstruction[]
						{
							new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand2, instr),
							new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr)
						});
					}
					else
					{
						bool flag4 = instr.OpCode == IROpCode.__XOR;
						if (flag4)
						{
							IRVariable tmp2 = tr.Context.AllocateVRegister(instr.Operand2.Type);
							IRVariable tmp3 = tr.Context.AllocateVRegister(instr.Operand2.Type);
							instrs.Replace(index, new IRInstruction[]
							{
								new IRInstruction(IROpCode.MOV, tmp2, instr.Operand1, instr),
								new IRInstruction(IROpCode.NOR, tmp2, instr.Operand2, instr),
								new IRInstruction(IROpCode.MOV, tmp3, instr.Operand2, instr),
								new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr),
								new IRInstruction(IROpCode.NOR, tmp3, tmp3, instr),
								new IRInstruction(IROpCode.NOR, instr.Operand1, tmp3, instr),
								new IRInstruction(IROpCode.NOR, instr.Operand1, tmp2, instr)
							});
						}
					}
				}
			}
		}
	}
}
