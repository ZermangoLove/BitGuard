using System;
using System.Reflection;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000FD RID: 253
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	internal enum ConstantFields
	{
		// Token: 0x040001A7 RID: 423
		E_CALL,
		// Token: 0x040001A8 RID: 424
		E_CALLVIRT,
		// Token: 0x040001A9 RID: 425
		E_NEWOBJ,
		// Token: 0x040001AA RID: 426
		E_CALLVIRT_CONSTRAINED,
		// Token: 0x040001AB RID: 427
		INIT,
		// Token: 0x040001AC RID: 428
		INSTANCE,
		// Token: 0x040001AD RID: 429
		CATCH,
		// Token: 0x040001AE RID: 430
		FILTER,
		// Token: 0x040001AF RID: 431
		FAULT,
		// Token: 0x040001B0 RID: 432
		FINALLY
	}
}
