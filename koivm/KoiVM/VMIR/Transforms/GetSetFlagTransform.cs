using System;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000AC RID: 172
	public class GetSetFlagTransform : ITransform
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000028A9 File Offset: 0x00000AA9
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F564 File Offset: 0x0000D764
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.__GETF;
			if (flag)
			{
				instrs.Replace(index, new IRInstruction[]
				{
					new IRInstruction(IROpCode.MOV, instr.Operand1, IRRegister.FL, instr),
					new IRInstruction(IROpCode.__AND, instr.Operand1, instr.Operand2, instr)
				});
			}
			else
			{
				bool flag2 = instr.OpCode == IROpCode.__SETF;
				if (flag2)
				{
					instrs.Replace(index, new IRInstruction[]
					{
						new IRInstruction(IROpCode.__OR, IRRegister.FL, instr.Operand1, instr)
					});
				}
			}
		}
	}
}
