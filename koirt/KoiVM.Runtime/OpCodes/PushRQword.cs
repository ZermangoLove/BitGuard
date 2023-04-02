using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000058 RID: 88
	internal class PushRQword : IOpCode
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000081D0 File Offset: 0x000063D0
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHR_QWORD;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000081E8 File Offset: 0x000063E8
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			byte regId = ctx.ReadByte();
			VMSlot slot = ctx.Registers[(int)regId];
			ctx.Stack[sp] = new VMSlot
			{
				U8 = slot.U8
			};
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
