using System;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000060 RID: 96
	internal class EHState
	{
		// Token: 0x04000014 RID: 20
		public EHState.EHProcess CurrentProcess;

		// Token: 0x04000015 RID: 21
		public object ExceptionObj;

		// Token: 0x04000016 RID: 22
		public VMSlot OldBP;

		// Token: 0x04000017 RID: 23
		public VMSlot OldSP;

		// Token: 0x04000018 RID: 24
		public int? CurrentFrame;

		// Token: 0x04000019 RID: 25
		public int? HandlerFrame;

		// Token: 0x0200007C RID: 124
		public enum EHProcess
		{
			// Token: 0x040000DC RID: 220
			Searching,
			// Token: 0x040000DD RID: 221
			Unwinding
		}
	}
}
