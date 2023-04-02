using System;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000064 RID: 100
	internal class StackRef : IReference
	{
		// Token: 0x06000138 RID: 312 RVA: 0x000087E0 File Offset: 0x000069E0
		public StackRef(uint pos)
		{
			this.StackPos = pos;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000087F2 File Offset: 0x000069F2
		// (set) Token: 0x0600013A RID: 314 RVA: 0x000087FA File Offset: 0x000069FA
		public uint StackPos { get; set; }

		// Token: 0x0600013B RID: 315 RVA: 0x00008804 File Offset: 0x00006A04
		public VMSlot GetValue(VMContext ctx, PointerType type)
		{
			VMSlot slot = ctx.Stack[this.StackPos];
			bool flag = type == PointerType.BYTE;
			if (flag)
			{
				slot.U8 = (ulong)slot.U1;
			}
			else
			{
				bool flag2 = type == PointerType.WORD;
				if (flag2)
				{
					slot.U8 = (ulong)slot.U2;
				}
				else
				{
					bool flag3 = type == PointerType.DWORD;
					if (flag3)
					{
						slot.U8 = (ulong)slot.U4;
					}
					else
					{
						bool flag4 = slot.O is IValueTypeBox;
						if (flag4)
						{
							slot.O = ((IValueTypeBox)slot.O).Clone();
						}
					}
				}
			}
			return slot;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000088AC File Offset: 0x00006AAC
		public void SetValue(VMContext ctx, VMSlot slot, PointerType type)
		{
			bool flag = type == PointerType.BYTE;
			if (flag)
			{
				slot.U8 = (ulong)slot.U1;
			}
			else
			{
				bool flag2 = type == PointerType.WORD;
				if (flag2)
				{
					slot.U8 = (ulong)slot.U2;
				}
				else
				{
					bool flag3 = type == PointerType.DWORD;
					if (flag3)
					{
						slot.U8 = (ulong)slot.U4;
					}
				}
			}
			ctx.Stack[this.StackPos] = slot;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000891C File Offset: 0x00006B1C
		public IReference Add(uint value)
		{
			return new StackRef(this.StackPos + value);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000893C File Offset: 0x00006B3C
		public IReference Add(ulong value)
		{
			return new StackRef(this.StackPos + (uint)value);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000895C File Offset: 0x00006B5C
		public void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type)
		{
			ctx.Stack.ToTypedReference(this.StackPos, typedRef, type);
		}
	}
}
