using System;

namespace KoiVM.VM
{
	// Token: 0x02000021 RID: 33
	public class VMDescriptor
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00006214 File Offset: 0x00004414
		public VMDescriptor(IVMSettings settings)
		{
			this.Random = new Random(settings.Seed);
			this.Settings = settings;
			this.Architecture = new ArchDescriptor(this.Random);
			this.Runtime = new RuntimeDescriptor(this.Random);
			this.Data = new DataDescriptor(this.Random);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000024AE File Offset: 0x000006AE
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000024B6 File Offset: 0x000006B6
		public Random Random { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000024BF File Offset: 0x000006BF
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000024C7 File Offset: 0x000006C7
		public IVMSettings Settings { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000024D0 File Offset: 0x000006D0
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000024D8 File Offset: 0x000006D8
		public ArchDescriptor Architecture { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000024E1 File Offset: 0x000006E1
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000024E9 File Offset: 0x000006E9
		public RuntimeDescriptor Runtime { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000024F2 File Offset: 0x000006F2
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000024FA File Offset: 0x000006FA
		public DataDescriptor Data { get; private set; }

		// Token: 0x060000BB RID: 187 RVA: 0x00002503 File Offset: 0x00000703
		public void ResetData()
		{
			this.Data = new DataDescriptor(this.Random);
		}
	}
}
