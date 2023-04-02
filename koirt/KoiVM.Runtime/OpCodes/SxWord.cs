using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200002B RID: 43
	internal class SxWord : IOpCode
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005764 File Offset: 0x00003964
		public byte Code
		{
			get
			{
				return Constants.OP_SX_WORD;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000577C File Offset: 0x0000397C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot operand = ctx.Stack[sp];
			bool flag = (operand.U2 & 32768) > 0;
			if (flag)
			{
				operand.U4 = (uint)operand.U2 | 4294901760U;
			}
			ctx.Stack[sp] = operand;
			state = ExecutionState.Next;
		}
	}
}
