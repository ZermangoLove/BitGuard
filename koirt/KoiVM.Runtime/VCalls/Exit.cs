using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000014 RID: 20
	internal class Exit : IVCall
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003B58 File Offset: 0x00001D58
		public byte Code
		{
			get
			{
				return Constants.VCALL_EXIT;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003B6F File Offset: 0x00001D6F
		public void Run(VMContext ctx, out ExecutionState state)
		{
			state = ExecutionState.Exit;
		}
	}
}
