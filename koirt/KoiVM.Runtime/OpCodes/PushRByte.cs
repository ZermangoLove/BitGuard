using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000055 RID: 85
	internal class PushRByte : IOpCode
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00007FA8 File Offset: 0x000061A8
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHR_BYTE;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007FC0 File Offset: 0x000061C0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			byte regId = ctx.ReadByte();
			VMSlot slot = ctx.Registers[(int)regId];
			ctx.Stack[sp] = new VMSlot
			{
				U1 = slot.U1
			};
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
