using System;
using System.Linq;
using KoiVM.VMIL;

namespace KoiVM.VM
{
	// Token: 0x02000016 RID: 22
	public class OpCodeDescriptor
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00005C24 File Offset: 0x00003E24
		public OpCodeDescriptor(Random random)
		{
			random.Shuffle(this.opCodeOrder);
		}

		// Token: 0x1700001C RID: 28
		public byte this[ILOpCode opCode]
		{
			get
			{
				return this.opCodeOrder[(int)opCode];
			}
		}

		// Token: 0x04000035 RID: 53
		private byte[] opCodeOrder = (from x in Enumerable.Range(0, 256)
			select (byte)x).ToArray<byte>();
	}
}
