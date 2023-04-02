using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000020 RID: 32
	internal class Call : IOpCode
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004FA0 File Offset: 0x000031A0
		public byte Code
		{
			get
			{
				return Constants.OP_CALL;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004FB8 File Offset: 0x000031B8
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot slot = ctx.Stack[sp];
			ctx.Stack[sp] = ctx.Registers[(int)Constants.REG_IP];
			ctx.Registers[(int)Constants.REG_IP].U8 = slot.U8;
			state = ExecutionState.Next;
		}
	}
}
