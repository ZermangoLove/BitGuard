using System;
using KoiVM.RT;

namespace KoiVM.AST.IL
{
	// Token: 0x0200013F RID: 319
	public class ILDataTarget : IILOperand, IHasOffset
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public ILDataTarget(BinaryChunk target)
		{
			this.Target = target;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00003BE6 File Offset: 0x00001DE6
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00003BEE File Offset: 0x00001DEE
		public BinaryChunk Target { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00003BF7 File Offset: 0x00001DF7
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00003BFF File Offset: 0x00001DFF
		public string Name { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001CEC0 File Offset: 0x0001B0C0
		public uint Offset
		{
			get
			{
				return this.Target.Offset;
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001CEE0 File Offset: 0x0001B0E0
		public override string ToString()
		{
			return this.Name;
		}
	}
}
