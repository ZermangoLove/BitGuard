using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000008 RID: 8
	internal class Ckfinite : IVCall
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public byte Code
		{
			get
			{
				return Constants.VCALL_CKFINITE;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot valueSlot = ctx.Stack[sp--];
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				float v = valueSlot.R4;
				bool flag2 = float.IsNaN(v) || float.IsInfinity(v);
				if (flag2)
				{
					throw new ArithmeticException();
				}
			}
			else
			{
				double v2 = valueSlot.R8;
				bool flag3 = double.IsNaN(v2) || double.IsInfinity(v2);
				if (flag3)
				{
					throw new ArithmeticException();
				}
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
