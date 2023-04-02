using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000022 RID: 34
	internal class IConvR64 : IOpCode
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000050DC File Offset: 0x000032DC
		public byte Code
		{
			get
			{
				return Constants.OP_ICONV_R64;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000050F4 File Offset: 0x000032F4
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp];
			double value = valueSlot.R8;
			valueSlot.U8 = (ulong)((long)value);
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1 & ~Constants.FL_OVERFLOW;
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				bool flag2 = value <= -1.0 || value >= 1.8446744073709552E+19;
				if (flag2)
				{
					fl |= Constants.FL_OVERFLOW;
				}
				bool flag3 = value >= 9.2233720368547758E+18;
				if (flag3)
				{
					valueSlot.U8 = (ulong)((double)((long)value) - 9.2233720368547758E+18) + 9223372036854775808UL;
				}
			}
			else
			{
				bool flag4 = value <= -9.2233720368547779E+18 || value >= 9.2233720368547758E+18;
				if (flag4)
				{
					fl |= Constants.FL_OVERFLOW;
				}
			}
			ctx.Registers[(int)Constants.REG_FL].U1 = fl & ~Constants.FL_UNSIGNED;
			ctx.Stack[sp] = valueSlot;
			state = ExecutionState.Next;
		}
	}
}
