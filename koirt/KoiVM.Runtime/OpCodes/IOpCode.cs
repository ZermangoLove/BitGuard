using System;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000054 RID: 84
	internal interface IOpCode
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000108 RID: 264
		byte Code { get; }

		// Token: 0x06000109 RID: 265
		void Run(VMContext ctx, out ExecutionState state);
	}
}
