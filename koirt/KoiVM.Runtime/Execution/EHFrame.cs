using System;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x0200005F RID: 95
	internal struct EHFrame
	{
		// Token: 0x0400000E RID: 14
		public byte EHType;

		// Token: 0x0400000F RID: 15
		public ulong FilterAddr;

		// Token: 0x04000010 RID: 16
		public ulong HandlerAddr;

		// Token: 0x04000011 RID: 17
		public Type CatchType;

		// Token: 0x04000012 RID: 18
		public VMSlot BP;

		// Token: 0x04000013 RID: 19
		public VMSlot SP;
	}
}
