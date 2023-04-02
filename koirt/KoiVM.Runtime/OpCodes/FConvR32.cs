using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000023 RID: 35
	internal class FConvR32 : IOpCode
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000522C File Offset: 0x0000342C
		public byte Code
		{
			get
			{
				return Constants.OP_FCONV_R32;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005244 File Offset: 0x00003444
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			valueSlot.R4 = (float)valueSlot.U8;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
