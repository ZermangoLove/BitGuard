using System;

namespace KoiVM.VM
{
	// Token: 0x0200001E RID: 30
	public class RuntimeDescriptor
	{
		// Token: 0x06000096 RID: 150 RVA: 0x0000241D File Offset: 0x0000061D
		public RuntimeDescriptor(Random random)
		{
			this.VMCall = new VMCallDescriptor(random);
			this.VCallOps = new VCallOpsDescriptor(random);
			this.RTFlags = new RTFlagDescriptor(random);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000244E File Offset: 0x0000064E
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002456 File Offset: 0x00000656
		public VMCallDescriptor VMCall { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000245F File Offset: 0x0000065F
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002467 File Offset: 0x00000667
		public VCallOpsDescriptor VCallOps { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002470 File Offset: 0x00000670
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00002478 File Offset: 0x00000678
		public RTFlagDescriptor RTFlags { get; private set; }
	}
}
