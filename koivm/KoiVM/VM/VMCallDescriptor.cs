using System;
using System.Linq;

namespace KoiVM.VM
{
	// Token: 0x0200001F RID: 31
	public class VMCallDescriptor
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00002481 File Offset: 0x00000681
		public VMCallDescriptor(Random random)
		{
			random.Shuffle(this.callOrder);
		}

		// Token: 0x1700002C RID: 44
		public int this[VMCalls call]
		{
			get
			{
				return this.callOrder[(int)call];
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00006038 File Offset: 0x00004238
		public int EXIT
		{
			get
			{
				return this.callOrder[0];
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00006054 File Offset: 0x00004254
		public int BREAK
		{
			get
			{
				return this.callOrder[1];
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00006070 File Offset: 0x00004270
		public int ECALL
		{
			get
			{
				return this.callOrder[2];
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000608C File Offset: 0x0000428C
		public int CAST
		{
			get
			{
				return this.callOrder[3];
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000060A8 File Offset: 0x000042A8
		public int CKFINITE
		{
			get
			{
				return this.callOrder[4];
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000060C4 File Offset: 0x000042C4
		public int CKOVERFLOW
		{
			get
			{
				return this.callOrder[5];
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000060E0 File Offset: 0x000042E0
		public int RANGECHK
		{
			get
			{
				return this.callOrder[6];
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000060FC File Offset: 0x000042FC
		public int INITOBJ
		{
			get
			{
				return this.callOrder[7];
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00006118 File Offset: 0x00004318
		public int LDFLD
		{
			get
			{
				return this.callOrder[8];
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00006134 File Offset: 0x00004334
		public int LDFTN
		{
			get
			{
				return this.callOrder[9];
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00006150 File Offset: 0x00004350
		public int TOKEN
		{
			get
			{
				return this.callOrder[10];
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000616C File Offset: 0x0000436C
		public int THROW
		{
			get
			{
				return this.callOrder[11];
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00006188 File Offset: 0x00004388
		public int SIZEOF
		{
			get
			{
				return this.callOrder[12];
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000061A4 File Offset: 0x000043A4
		public int STFLD
		{
			get
			{
				return this.callOrder[13];
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000061C0 File Offset: 0x000043C0
		public int BOX
		{
			get
			{
				return this.callOrder[14];
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000061DC File Offset: 0x000043DC
		public int UNBOX
		{
			get
			{
				return this.callOrder[15];
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000061F8 File Offset: 0x000043F8
		public int LOCALLOC
		{
			get
			{
				return this.callOrder[16];
			}
		}

		// Token: 0x0400006A RID: 106
		private int[] callOrder = Enumerable.Range(0, 256).ToArray<int>();
	}
}
