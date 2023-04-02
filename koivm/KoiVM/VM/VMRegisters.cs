using System;
using System.Reflection;

namespace KoiVM.VM
{
	// Token: 0x02000019 RID: 25
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum VMRegisters
	{
		// Token: 0x04000043 RID: 67
		R0,
		// Token: 0x04000044 RID: 68
		R1,
		// Token: 0x04000045 RID: 69
		R2,
		// Token: 0x04000046 RID: 70
		R3,
		// Token: 0x04000047 RID: 71
		R4,
		// Token: 0x04000048 RID: 72
		R5,
		// Token: 0x04000049 RID: 73
		R6,
		// Token: 0x0400004A RID: 74
		R7,
		// Token: 0x0400004B RID: 75
		BP,
		// Token: 0x0400004C RID: 76
		SP,
		// Token: 0x0400004D RID: 77
		IP,
		// Token: 0x0400004E RID: 78
		FL,
		// Token: 0x0400004F RID: 79
		K1,
		// Token: 0x04000050 RID: 80
		K2,
		// Token: 0x04000051 RID: 81
		M1,
		// Token: 0x04000052 RID: 82
		M2,
		// Token: 0x04000053 RID: 83
		Max
	}
}
