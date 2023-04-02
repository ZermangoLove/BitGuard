using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000C RID: 12
	internal class Localloc : IVCall
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000030C4 File Offset: 0x000012C4
		public byte Code
		{
			get
			{
				return Constants.VCALL_LOCALLOC;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000030DC File Offset: 0x000012DC
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			uint bp = ctx.Registers[(int)Constants.REG_BP].U4;
			uint size = ctx.Stack[sp].U4;
			ctx.Stack[sp] = new VMSlot
			{
				U8 = (ulong)(long)ctx.Stack.Localloc(bp, size)
			};
			state = ExecutionState.Next;
		}
	}
}
