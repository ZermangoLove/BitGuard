using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000024 RID: 36
	internal class FConvR64 : IOpCode
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00005298 File Offset: 0x00003498
		public byte Code
		{
			get
			{
				return Constants.OP_FCONV_R64;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000052B0 File Offset: 0x000034B0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				valueSlot.R8 = valueSlot.U8;
			}
			else
			{
				valueSlot.R8 = (double)valueSlot.U8;
			}
			ctx.Registers[(int)Constants.REG_FL].U1 = fl & ~Constants.FL_UNSIGNED;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
