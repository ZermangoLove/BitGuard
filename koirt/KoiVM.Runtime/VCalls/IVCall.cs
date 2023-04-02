using System;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000016 RID: 22
	internal interface IVCall
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004E RID: 78
		byte Code { get; }

		// Token: 0x0600004F RID: 79
		void Run(VMContext ctx, out ExecutionState state);
	}
}
