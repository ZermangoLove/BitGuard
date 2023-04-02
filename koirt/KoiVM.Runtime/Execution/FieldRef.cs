using System;
using System.Reflection;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x0200005D RID: 93
	internal class FieldRef : IReference
	{
		// Token: 0x06000125 RID: 293 RVA: 0x000084DE File Offset: 0x000066DE
		public FieldRef(object instance, FieldInfo field)
		{
			this.instance = instance;
			this.field = field;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000084F8 File Offset: 0x000066F8
		public VMSlot GetValue(VMContext ctx, PointerType type)
		{
			object inst = this.instance;
			bool flag = this.field.DeclaringType.IsValueType && this.instance is IReference;
			if (flag)
			{
				inst = ((IReference)this.instance).GetValue(ctx, PointerType.OBJECT).ToObject(this.field.DeclaringType);
			}
			return VMSlot.FromObject(this.field.GetValue(inst), this.field.FieldType);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000857C File Offset: 0x0000677C
		public unsafe void SetValue(VMContext ctx, VMSlot slot, PointerType type)
		{
			bool flag = this.field.DeclaringType.IsValueType && this.instance is IReference;
			if (flag)
			{
				TypedReference typedRef;
				((IReference)this.instance).ToTypedReference(ctx, (void*)(&typedRef), this.field.DeclaringType);
				this.field.SetValueDirect(typedRef, slot.ToObject(this.field.FieldType));
			}
			else
			{
				this.field.SetValue(this.instance, slot.ToObject(this.field.FieldType));
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008620 File Offset: 0x00006820
		public IReference Add(uint value)
		{
			return this;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008634 File Offset: 0x00006834
		public IReference Add(ulong value)
		{
			return this;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008647 File Offset: 0x00006847
		public void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type)
		{
			TypedReferenceHelpers.GetFieldAddr(ctx, this.instance, this.field, typedRef);
		}

		// Token: 0x0400000A RID: 10
		private object instance;

		// Token: 0x0400000B RID: 11
		private FieldInfo field;
	}
}
