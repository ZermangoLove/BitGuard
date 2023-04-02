using System;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000062 RID: 98
	internal interface IReference
	{
		// Token: 0x06000133 RID: 307
		VMSlot GetValue(VMContext ctx, PointerType type);

		// Token: 0x06000134 RID: 308
		void SetValue(VMContext ctx, VMSlot slot, PointerType type);

		// Token: 0x06000135 RID: 309
		IReference Add(uint value);

		// Token: 0x06000136 RID: 310
		IReference Add(ulong value);

		// Token: 0x06000137 RID: 311
		void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type);
	}
}
