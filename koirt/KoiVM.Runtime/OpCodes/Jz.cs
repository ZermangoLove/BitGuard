using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000050 RID: 80
	internal class Jz : IOpCode
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00007C94 File Offset: 0x00005E94
		public byte Code
		{
			get
			{
				return Constants.OP_JZ;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007CAC File Offset: 0x00005EAC
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			VMSlot valSlot = ctx.Stack[sp - 1U];
			sp -= 2U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = valSlot.U8 == 0UL;
			if (flag)
			{
				ctx.Registers[(int)Constants.REG_IP].U8 = adrSlot.U8;
			}
			state = ExecutionState.Next;
		}
	}
}
