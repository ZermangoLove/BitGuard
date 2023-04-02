using System;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x0200005E RID: 94
	internal class TypedRef : IReference
	{
		// Token: 0x0600012B RID: 299 RVA: 0x0000865E File Offset: 0x0000685E
		public TypedRef(TypedRefPtr ptr)
		{
			this._ptr = new TypedRefPtr?(ptr);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008674 File Offset: 0x00006874
		public unsafe TypedRef(TypedReference typedRef)
		{
			this._ptr = null;
			this._typedRef = *(TypedRef.PseudoTypedRef*)(&typedRef);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008698 File Offset: 0x00006898
		public unsafe VMSlot GetValue(VMContext ctx, PointerType type)
		{
			bool flag = this._ptr != null;
			TypedReference typedRef;
			if (flag)
			{
				*(&typedRef) = *(TypedReference*)this._ptr.Value;
			}
			else
			{
				*(TypedRef.PseudoTypedRef*)(&typedRef) = this._typedRef;
			}
			return VMSlot.FromObject(TypedReference.ToObject(typedRef), __reftype(typedRef));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000086FC File Offset: 0x000068FC
		public unsafe void SetValue(VMContext ctx, VMSlot slot, PointerType type)
		{
			bool flag = this._ptr != null;
			TypedReference typedRef;
			if (flag)
			{
				*(&typedRef) = *(TypedReference*)this._ptr.Value;
			}
			else
			{
				*(TypedRef.PseudoTypedRef*)(&typedRef) = this._typedRef;
			}
			Type refType = __reftype(typedRef);
			object value = slot.ToObject(refType);
			TypedReferenceHelpers.SetTypedRef(value, (void*)(&typedRef));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008768 File Offset: 0x00006968
		public IReference Add(uint value)
		{
			return this;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000877C File Offset: 0x0000697C
		public IReference Add(ulong value)
		{
			return this;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008790 File Offset: 0x00006990
		public unsafe void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type)
		{
			bool flag = this._ptr != null;
			if (flag)
			{
				*(TypedReference*)typedRef = *(TypedReference*)this._ptr.Value;
			}
			else
			{
				*(TypedRef.PseudoTypedRef*)typedRef = this._typedRef;
			}
		}

		// Token: 0x0400000C RID: 12
		private TypedRefPtr? _ptr;

		// Token: 0x0400000D RID: 13
		private TypedRef.PseudoTypedRef _typedRef;

		// Token: 0x0200007B RID: 123
		private struct PseudoTypedRef
		{
			// Token: 0x040000D9 RID: 217
			public IntPtr Type;

			// Token: 0x040000DA RID: 218
			public IntPtr Value;
		}
	}
}
