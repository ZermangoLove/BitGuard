using System;

namespace KoiVM.CFG
{
	// Token: 0x0200011C RID: 284
	[Flags]
	public enum BlockFlags
	{
		// Token: 0x040001FE RID: 510
		Normal = 0,
		// Token: 0x040001FF RID: 511
		ExitEHLeave = 1,
		// Token: 0x04000200 RID: 512
		ExitEHReturn = 2
	}
}
