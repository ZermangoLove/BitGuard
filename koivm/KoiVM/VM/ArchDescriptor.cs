using System;

namespace KoiVM.VM
{
	// Token: 0x0200001A RID: 26
	public class ArchDescriptor
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00002313 File Offset: 0x00000513
		public ArchDescriptor(Random random)
		{
			this.OpCodes = new OpCodeDescriptor(random);
			this.Flags = new FlagDescriptor(random);
			this.Registers = new RegisterDescriptor(random);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002344 File Offset: 0x00000544
		// (set) Token: 0x0600007D RID: 125 RVA: 0x0000234C File Offset: 0x0000054C
		public OpCodeDescriptor OpCodes { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002355 File Offset: 0x00000555
		// (set) Token: 0x0600007F RID: 127 RVA: 0x0000235D File Offset: 0x0000055D
		public FlagDescriptor Flags { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002366 File Offset: 0x00000566
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000236E File Offset: 0x0000056E
		public RegisterDescriptor Registers { get; private set; }
	}
}
