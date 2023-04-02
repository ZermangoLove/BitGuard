using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000025 RID: 37
	internal class FConvR32R64 : IOpCode
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000535C File Offset: 0x0000355C
		public byte Code
		{
			get
			{
				return Constants.OP_FCONV_R32_R64;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005374 File Offset: 0x00003574
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			valueSlot.R8 = (double)valueSlot.R4;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
