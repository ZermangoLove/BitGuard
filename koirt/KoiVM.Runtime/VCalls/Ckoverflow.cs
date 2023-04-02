using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000009 RID: 9
	internal class Ckoverflow : IVCall
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002EC8 File Offset: 0x000010C8
		public byte Code
		{
			get
			{
				return Constants.VCALL_CKOVERFLOW;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002EE0 File Offset: 0x000010E0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			bool flag = ctx.Stack[sp--].U4 > 0U;
			if (flag)
			{
				throw new OverflowException();
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
