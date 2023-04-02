using System;
using System.Reflection;

namespace KoiVM.VM
{
	// Token: 0x02000020 RID: 32
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum VMCalls
	{
		// Token: 0x0400006C RID: 108
		EXIT,
		// Token: 0x0400006D RID: 109
		BREAK,
		// Token: 0x0400006E RID: 110
		ECALL,
		// Token: 0x0400006F RID: 111
		CAST,
		// Token: 0x04000070 RID: 112
		CKFINITE,
		// Token: 0x04000071 RID: 113
		CKOVERFLOW,
		// Token: 0x04000072 RID: 114
		RANGECHK,
		// Token: 0x04000073 RID: 115
		INITOBJ,
		// Token: 0x04000074 RID: 116
		LDFLD,
		// Token: 0x04000075 RID: 117
		LDFTN,
		// Token: 0x04000076 RID: 118
		TOKEN,
		// Token: 0x04000077 RID: 119
		THROW,
		// Token: 0x04000078 RID: 120
		SIZEOF,
		// Token: 0x04000079 RID: 121
		STFLD,
		// Token: 0x0400007A RID: 122
		BOX,
		// Token: 0x0400007B RID: 123
		UNBOX,
		// Token: 0x0400007C RID: 124
		LOCALLOC,
		// Token: 0x0400007D RID: 125
		Max
	}
}
