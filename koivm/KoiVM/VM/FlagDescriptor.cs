using System;
using System.Linq;

namespace KoiVM.VM
{
	// Token: 0x0200001D RID: 29
	public class FlagDescriptor
	{
		// Token: 0x0600008C RID: 140 RVA: 0x000023F4 File Offset: 0x000005F4
		public FlagDescriptor(Random random)
		{
			random.Shuffle(this.flagOrder);
		}

		// Token: 0x17000020 RID: 32
		public int this[VMFlags flag]
		{
			get
			{
				return this.flagOrder[(int)flag];
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00005F3C File Offset: 0x0000413C
		public int OVERFLOW
		{
			get
			{
				return this.flagOrder[0];
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00005F58 File Offset: 0x00004158
		public int CARRY
		{
			get
			{
				return this.flagOrder[1];
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00005F74 File Offset: 0x00004174
		public int ZERO
		{
			get
			{
				return this.flagOrder[2];
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00005F90 File Offset: 0x00004190
		public int SIGN
		{
			get
			{
				return this.flagOrder[3];
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00005FAC File Offset: 0x000041AC
		public int UNSIGNED
		{
			get
			{
				return this.flagOrder[4];
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005FC8 File Offset: 0x000041C8
		public int BEHAV1
		{
			get
			{
				return this.flagOrder[5];
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00005FE4 File Offset: 0x000041E4
		public int BEHAV2
		{
			get
			{
				return this.flagOrder[6];
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00006000 File Offset: 0x00004200
		public int BEHAV3
		{
			get
			{
				return this.flagOrder[7];
			}
		}

		// Token: 0x04000066 RID: 102
		private int[] flagOrder = Enumerable.Range(0, 8).ToArray<int>();
	}
}
