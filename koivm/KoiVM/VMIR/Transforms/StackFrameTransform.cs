using System;
using KoiVM.AST.IR;
using KoiVM.VMIR.RegAlloc;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000AA RID: 170
	public class StackFrameTransform : ITransform
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000281C File Offset: 0x00000A1C
		public void Initialize(IRTransformer tr)
		{
			this.allocator = (RegisterAllocator)tr.Annotations[RegisterAllocationTransform.RegAllocatorKey];
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000283A File Offset: 0x00000A3A
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F484 File Offset: 0x0000D684
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.__ENTRY && !this.doneEntry;
			if (flag)
			{
				instrs.Replace(index, new IRInstruction[]
				{
					instr,
					new IRInstruction(IROpCode.PUSH, IRRegister.BP),
					new IRInstruction(IROpCode.MOV, IRRegister.BP, IRRegister.SP),
					new IRInstruction(IROpCode.ADD, IRRegister.SP, IRConstant.FromI4(this.allocator.LocalSize))
				});
				this.doneEntry = true;
			}
			else
			{
				bool flag2 = instr.OpCode == IROpCode.__EXIT && !this.doneExit;
				if (flag2)
				{
					instrs.Replace(index, new IRInstruction[]
					{
						new IRInstruction(IROpCode.MOV, IRRegister.SP, IRRegister.BP),
						new IRInstruction(IROpCode.POP, IRRegister.BP),
						instr
					});
					this.doneExit = true;
				}
			}
		}

		// Token: 0x040000E9 RID: 233
		private RegisterAllocator allocator;

		// Token: 0x040000EA RID: 234
		private bool doneEntry;

		// Token: 0x040000EB RID: 235
		private bool doneExit;
	}
}
