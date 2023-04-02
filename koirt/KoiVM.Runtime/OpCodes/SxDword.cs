using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200002A RID: 42
	internal class SxDword : IOpCode
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000056DC File Offset: 0x000038DC
		public byte Code
		{
			get
			{
				return Constants.OP_SX_DWORD;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000056F4 File Offset: 0x000038F4
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot operand = ctx.Stack[sp];
			bool flag = (operand.U4 & 2147483648U) > 0U;
			if (flag)
			{
				operand.U8 = 18446744069414584320UL | (ulong)operand.U4;
			}
			ctx.Stack[sp] = operand;
			state = ExecutionState.Next;
		}
	}
}
