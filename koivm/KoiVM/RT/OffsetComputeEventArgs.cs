using System;

namespace KoiVM.RT
{
	// Token: 0x020000EB RID: 235
	public class OffsetComputeEventArgs : EventArgs
	{
		// Token: 0x06000383 RID: 899 RVA: 0x00002EEB File Offset: 0x000010EB
		internal OffsetComputeEventArgs(uint offset)
		{
			this.Offset = offset;
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00002EFD File Offset: 0x000010FD
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00002F05 File Offset: 0x00001105
		public uint Offset { get; private set; }
	}
}
