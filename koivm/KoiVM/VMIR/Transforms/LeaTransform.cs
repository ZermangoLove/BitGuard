using System;
using System.Diagnostics;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A5 RID: 165
	public class LeaTransform : ITransform
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000278D File Offset: 0x0000098D
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.__LEA;
			if (flag)
			{
				IRPointer source = (IRPointer)instr.Operand2;
				IIROperand target = instr.Operand1;
				Debug.Assert(source.Register == IRRegister.BP);
				instrs.Replace(index, new IRInstruction[]
				{
					new IRInstruction(IROpCode.MOV, target, IRRegister.BP, instr),
					new IRInstruction(IROpCode.ADD, target, IRConstant.FromI4(source.Offset), instr)
				});
			}
		}
	}
}
