using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000059 RID: 89
	internal class PushRObject : IOpCode
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00008270 File Offset: 0x00006470
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHR_OBJECT;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008288 File Offset: 0x00006488
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			byte regId = ctx.ReadByte();
			VMSlot slot = ctx.Registers[(int)regId];
			ctx.Stack[sp] = slot;
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
