using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200002C RID: 44
	internal class SxByte : IOpCode
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000057E8 File Offset: 0x000039E8
		public byte Code
		{
			get
			{
				return Constants.OP_SX_BYTE;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005800 File Offset: 0x00003A00
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot operand = ctx.Stack[sp];
			bool flag = (operand.U1 & 128) > 0;
			if (flag)
			{
				operand.U4 = (uint)operand.U1 | 4294967040U;
			}
			ctx.Stack[sp] = operand;
			state = ExecutionState.Next;
		}
	}
}
