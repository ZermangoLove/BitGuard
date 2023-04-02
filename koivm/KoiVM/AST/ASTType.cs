using System;
using System.Reflection;

namespace KoiVM.AST
{
	// Token: 0x0200012A RID: 298
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum ASTType
	{
		// Token: 0x0400021B RID: 539
		I4,
		// Token: 0x0400021C RID: 540
		I8,
		// Token: 0x0400021D RID: 541
		R4,
		// Token: 0x0400021E RID: 542
		R8,
		// Token: 0x0400021F RID: 543
		O,
		// Token: 0x04000220 RID: 544
		Ptr,
		// Token: 0x04000221 RID: 545
		ByRef
	}
}
