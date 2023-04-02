using System;

namespace KoiVM.Runtime
{
	// Token: 0x02000002 RID: 2
	internal static class Platform
	{
		// Token: 0x04000001 RID: 1
		public static readonly bool x64 = IntPtr.Size == 8;

		// Token: 0x04000002 RID: 2
		public static readonly bool LittleEndian = BitConverter.IsLittleEndian;
	}
}
