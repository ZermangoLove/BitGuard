using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000042 RID: 66
	internal class Ret : IOpCode
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000727C File Offset: 0x0000547C
		public byte Code
		{
			get
			{
				return Constants.OP_RET;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007294 File Offset: 0x00005494
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot slot = ctx.Stack[sp];
			ctx.Stack.SetTopPosition(sp -= 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			ctx.Registers[(int)Constants.REG_IP].U8 = slot.U8;
			state = ExecutionState.Next;
		}
	}
}
