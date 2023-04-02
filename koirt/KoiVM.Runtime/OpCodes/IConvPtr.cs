using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000021 RID: 33
	internal class IConvPtr : IOpCode
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00005028 File Offset: 0x00003228
		public byte Code
		{
			get
			{
				return Constants.OP_ICONV_PTR;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005040 File Offset: 0x00003240
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1 & ~Constants.FL_OVERFLOW;
			bool flag = !Platform.x64 && valueSlot.U8 >> 32 > 0UL;
			if (flag)
			{
				fl |= Constants.FL_OVERFLOW;
			}
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
