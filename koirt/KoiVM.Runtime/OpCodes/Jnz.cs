using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000051 RID: 81
	internal class Jnz : IOpCode
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00007D44 File Offset: 0x00005F44
		public byte Code
		{
			get
			{
				return Constants.OP_JNZ;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007D5C File Offset: 0x00005F5C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			VMSlot valSlot = ctx.Stack[sp - 1U];
			sp -= 2U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = valSlot.U8 > 0UL;
			if (flag)
			{
				ctx.Registers[(int)Constants.REG_IP].U8 = adrSlot.U8;
			}
			state = ExecutionState.Next;
		}
	}
}
