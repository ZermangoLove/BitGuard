using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000A RID: 10
	internal class Rangechk : IVCall
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002F50 File Offset: 0x00001150
		public byte Code
		{
			get
			{
				return Constants.VCALL_RANGECHK;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F68 File Offset: 0x00001168
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp--];
			VMSlot maxSlot = ctx.Stack[sp--];
			VMSlot minSlot = ctx.Stack[sp];
			valueSlot.U8 = ((valueSlot.U8 > maxSlot.U8 || valueSlot.U8 < minSlot.U8) ? 1UL : 0UL);
			ctx.Stack[sp] = valueSlot;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
