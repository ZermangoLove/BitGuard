using System;
using System.Linq;

namespace KoiVM.VM
{
	// Token: 0x02000010 RID: 16
	public class RTFlagDescriptor
	{
		// Token: 0x0600005F RID: 95 RVA: 0x000058EC File Offset: 0x00003AEC
		public RTFlagDescriptor(Random random)
		{
			random.Shuffle(this.flagOrder);
			random.Shuffle(this.ehOrder);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00005988 File Offset: 0x00003B88
		public byte INSTANCE
		{
			get
			{
				return this.flagOrder[0];
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000059A4 File Offset: 0x00003BA4
		public byte EH_CATCH
		{
			get
			{
				return this.ehOrder[0];
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000059C0 File Offset: 0x00003BC0
		public byte EH_FILTER
		{
			get
			{
				return this.ehOrder[1];
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000059DC File Offset: 0x00003BDC
		public byte EH_FAULT
		{
			get
			{
				return this.ehOrder[2];
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000059F8 File Offset: 0x00003BF8
		public byte EH_FINALLY
		{
			get
			{
				return this.ehOrder[3];
			}
		}

		// Token: 0x04000029 RID: 41
		private byte[] flagOrder = (from x in Enumerable.Range(1, 7)
			select (byte)x).ToArray<byte>();

		// Token: 0x0400002A RID: 42
		private byte[] ehOrder = (from x in Enumerable.Range(0, 4)
			select (byte)x).ToArray<byte>();
	}
}
