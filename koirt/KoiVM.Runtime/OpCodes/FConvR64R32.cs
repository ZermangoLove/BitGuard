using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000026 RID: 38
	internal class FConvR64R32 : IOpCode
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000053C8 File Offset: 0x000035C8
		public byte Code
		{
			get
			{
				return Constants.OP_FCONV_R64_R32;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000053E0 File Offset: 0x000035E0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			valueSlot.R4 = (float)valueSlot.R8;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
