using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000029 RID: 41
	internal class Nop : IOpCode
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000056BC File Offset: 0x000038BC
		public byte Code
		{
			get
			{
				return Constants.OP_NOP;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000056D3 File Offset: 0x000038D3
		public void Run(VMContext ctx, out ExecutionState state)
		{
			state = ExecutionState.Next;
		}
	}
}
