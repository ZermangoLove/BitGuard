using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000F RID: 15
	internal class Throw : IVCall
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000033A0 File Offset: 0x000015A0
		public byte Code
		{
			get
			{
				return Constants.VCALL_THROW;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033B8 File Offset: 0x000015B8
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			uint type = ctx.Stack[sp--].U4;
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = type == 1U;
			if (flag)
			{
				state = ExecutionState.Rethrow;
			}
			else
			{
				state = ExecutionState.Throw;
			}
		}
	}
}
