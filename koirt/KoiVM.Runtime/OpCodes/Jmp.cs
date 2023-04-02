using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004F RID: 79
	internal class Jmp : IOpCode
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00007C00 File Offset: 0x00005E00
		public byte Code
		{
			get
			{
				return Constants.OP_JMP;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007C18 File Offset: 0x00005E18
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot slot = ctx.Stack[sp];
			ctx.Stack.SetTopPosition(sp -= 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			ctx.Registers[(int)Constants.REG_IP].U8 = slot.U8;
			state = ExecutionState.Next;
		}
	}
}
