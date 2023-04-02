using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000053 RID: 83
	internal class Pop : IOpCode
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007EC8 File Offset: 0x000060C8
		public byte Code
		{
			get
			{
				return Constants.OP_POP;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007EE0 File Offset: 0x000060E0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot slot = ctx.Stack[sp];
			ctx.Stack.SetTopPosition(sp -= 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			byte regId = ctx.ReadByte();
			bool flag = (regId == Constants.REG_SP || regId == Constants.REG_BP) && slot.O is StackRef;
			if (flag)
			{
				ctx.Registers[(int)regId] = new VMSlot
				{
					U4 = ((StackRef)slot.O).StackPos
				};
			}
			else
			{
				ctx.Registers[(int)regId] = slot;
			}
			state = ExecutionState.Next;
		}
	}
}
