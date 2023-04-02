using System;
using KoiVM.Runtime.Data;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.VCalls;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200003B RID: 59
	internal class Vcall : IOpCode
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00006ABC File Offset: 0x00004CBC
		public byte Code
		{
			get
			{
				return Constants.OP_VCALL;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot slot = ctx.Stack[sp];
			ctx.Stack.SetTopPosition(sp -= 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			IVCall vCall = VCallMap.Lookup(slot.U1);
			vCall.Run(ctx, out state);
		}
	}
}
