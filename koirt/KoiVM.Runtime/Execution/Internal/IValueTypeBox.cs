using System;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006B RID: 107
	internal interface IValueTypeBox
	{
		// Token: 0x06000170 RID: 368
		object GetValue();

		// Token: 0x06000171 RID: 369
		Type GetValueType();

		// Token: 0x06000172 RID: 370
		IValueTypeBox Clone();
	}
}
