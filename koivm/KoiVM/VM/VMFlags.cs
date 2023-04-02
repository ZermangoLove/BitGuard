using System;
using System.Reflection;

namespace KoiVM.VM
{
	// Token: 0x02000018 RID: 24
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum VMFlags
	{
		// Token: 0x04000039 RID: 57
		OVERFLOW,
		// Token: 0x0400003A RID: 58
		CARRY,
		// Token: 0x0400003B RID: 59
		ZERO,
		// Token: 0x0400003C RID: 60
		SIGN,
		// Token: 0x0400003D RID: 61
		UNSIGNED,
		// Token: 0x0400003E RID: 62
		BEHAV1,
		// Token: 0x0400003F RID: 63
		BEHAV2,
		// Token: 0x04000040 RID: 64
		BEHAV3,
		// Token: 0x04000041 RID: 65
		Max
	}
}
