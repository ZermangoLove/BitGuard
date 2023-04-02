using System;
using System.Linq;

namespace KoiVM.VM
{
	// Token: 0x02000014 RID: 20
	public class RegisterDescriptor
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00005BAC File Offset: 0x00003DAC
		public RegisterDescriptor(Random random)
		{
			random.Shuffle(this.regOrder);
		}

		// Token: 0x1700001B RID: 27
		public byte this[VMRegisters reg]
		{
			get
			{
				return this.regOrder[(int)reg];
			}
		}

		// Token: 0x04000032 RID: 50
		private byte[] regOrder = (from x in Enumerable.Range(0, 16)
			select (byte)x).ToArray<byte>();
	}
}
