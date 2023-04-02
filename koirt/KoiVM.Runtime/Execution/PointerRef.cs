using System;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x0200005C RID: 92
	internal class PointerRef : IReference
	{
		// Token: 0x0600011F RID: 287 RVA: 0x000084B4 File Offset: 0x000066B4
		public unsafe PointerRef(void* ptr)
		{
			this.ptr = ptr;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000084C5 File Offset: 0x000066C5
		public VMSlot GetValue(VMContext ctx, PointerType type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000084C5 File Offset: 0x000066C5
		public void SetValue(VMContext ctx, VMSlot slot, PointerType type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000084C5 File Offset: 0x000066C5
		public IReference Add(uint value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000084C5 File Offset: 0x000066C5
		public IReference Add(ulong value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000084CD File Offset: 0x000066CD
		public void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type)
		{
			TypedReferenceHelpers.MakeTypedRef(this.ptr, typedRef, type);
		}

		// Token: 0x04000009 RID: 9
		private unsafe void* ptr;
	}
}
