using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000056 RID: 86
	internal class PushRWord : IOpCode
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00008048 File Offset: 0x00006248
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHR_WORD;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008060 File Offset: 0x00006260
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			byte regId = ctx.ReadByte();
			VMSlot slot = ctx.Registers[(int)regId];
			ctx.Stack[sp] = new VMSlot
			{
				U2 = slot.U2
			};
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
