using System;

namespace KoiVM.VM
{
	// Token: 0x02000012 RID: 18
	public class VCallOpsDescriptor
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000022CD File Offset: 0x000004CD
		public VCallOpsDescriptor(Random random)
		{
			random.Shuffle(this.ecallOrder);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00005A14 File Offset: 0x00003C14
		public uint ECALL_CALL
		{
			get
			{
				return this.ecallOrder[0];
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00005A30 File Offset: 0x00003C30
		public uint ECALL_CALLVIRT
		{
			get
			{
				return this.ecallOrder[1];
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00005A4C File Offset: 0x00003C4C
		public uint ECALL_NEWOBJ
		{
			get
			{
				return this.ecallOrder[2];
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00005A68 File Offset: 0x00003C68
		public uint ECALL_CALLVIRT_CONSTRAINED
		{
			get
			{
				return this.ecallOrder[3];
			}
		}

		// Token: 0x0400002E RID: 46
		private uint[] ecallOrder = new uint[] { 0U, 1U, 2U, 3U };
	}
}
