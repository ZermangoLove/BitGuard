using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000069 RID: 105
	internal class VMStack
	{
		// Token: 0x17000060 RID: 96
		public VMSlot this[uint pos]
		{
			get
			{
				bool flag = pos > this.topPos;
				VMSlot vmslot;
				if (flag)
				{
					vmslot = VMSlot.Null;
				}
				else
				{
					uint sectionIndex = pos >> 6;
					vmslot = this.sections[(int)sectionIndex][(int)(pos & 63U)];
				}
				return vmslot;
			}
			set
			{
				bool flag = pos > this.topPos;
				if (!flag)
				{
					uint sectionIndex = pos >> 6;
					this.sections[(int)sectionIndex][(int)(pos & 63U)] = value;
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009C70 File Offset: 0x00007E70
		public void SetTopPosition(uint topPos)
		{
			bool flag = topPos > 2147483647U;
			if (flag)
			{
				throw new StackOverflowException();
			}
			uint sectionIndex = topPos >> 6;
			bool flag2 = (ulong)sectionIndex >= (ulong)((long)this.sections.Count);
			if (flag2)
			{
				do
				{
					this.sections.Add(new VMSlot[64]);
				}
				while ((ulong)sectionIndex >= (ulong)((long)this.sections.Count));
			}
			else
			{
				bool flag3 = (ulong)sectionIndex < (ulong)((long)(this.sections.Count - 2));
				if (flag3)
				{
					do
					{
						this.sections.RemoveAt(this.sections.Count - 1);
					}
					while ((ulong)sectionIndex < (ulong)((long)(this.sections.Count - 2)));
				}
			}
			uint stackIndex = (topPos & 63U) + 1U;
			VMSlot[] section = this.sections[(int)sectionIndex];
			while ((ulong)stackIndex < (ulong)((long)section.Length) && section[(int)stackIndex].O != null)
			{
				section[(int)stackIndex++] = VMSlot.Null;
			}
			bool flag4 = (ulong)stackIndex == (ulong)((long)section.Length) && (ulong)(sectionIndex + 1U) < (ulong)((long)this.sections.Count);
			if (flag4)
			{
				stackIndex = 0U;
				section = this.sections[(int)(sectionIndex + 1U)];
				while ((ulong)stackIndex < (ulong)((long)section.Length) && section[(int)stackIndex].O != null)
				{
					section[(int)stackIndex++] = VMSlot.Null;
				}
			}
			this.topPos = topPos;
			this.CheckFreeLocalloc();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009DE8 File Offset: 0x00007FE8
		private void CheckFreeLocalloc()
		{
			while (this.localPool != null && this.localPool.GuardPos > this.topPos)
			{
				this.localPool = this.localPool.Free();
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009E2C File Offset: 0x0000802C
		public IntPtr Localloc(uint guardPos, uint size)
		{
			VMStack.LocallocNode node = new VMStack.LocallocNode
			{
				GuardPos = guardPos,
				Memory = Marshal.AllocHGlobal((int)size)
			};
			VMStack.LocallocNode insert;
			for (insert = this.localPool; insert != null; insert = insert.Next)
			{
				bool flag = insert.Next == null || insert.Next.GuardPos < guardPos;
				if (flag)
				{
					break;
				}
			}
			bool flag2 = insert == null;
			if (flag2)
			{
				this.localPool = node;
			}
			else
			{
				node.Next = insert.Next;
				insert.Next = node;
			}
			return node.Memory;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009EC0 File Offset: 0x000080C0
		public void FreeAllLocalloc()
		{
			for (VMStack.LocallocNode node = this.localPool; node != null; node = node.Free())
			{
			}
			this.localPool = null;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009EF0 File Offset: 0x000080F0
		~VMStack()
		{
			this.FreeAllLocalloc();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009F20 File Offset: 0x00008120
		public void ToTypedReference(uint pos, TypedRefPtr typedRef, Type type)
		{
			bool flag = pos > this.topPos;
			if (flag)
			{
				throw new ExecutionEngineException();
			}
			VMSlot[] section = this.sections[(int)(pos >> 6)];
			uint index = pos & 63U;
			bool isEnum = type.IsEnum;
			if (isEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			bool flag2 = type.IsPrimitive || type.IsPointer;
			if (flag2)
			{
				section[(int)index].ToTypedReferencePrimitive(typedRef);
				TypedReferenceHelpers.CastTypedRef(typedRef, type);
			}
			else
			{
				section[(int)index].ToTypedReferenceObject(typedRef, type);
			}
		}

		// Token: 0x04000036 RID: 54
		private const int SectionSize = 6;

		// Token: 0x04000037 RID: 55
		private const int IndexMask = 63;

		// Token: 0x04000038 RID: 56
		private List<VMSlot[]> sections = new List<VMSlot[]>();

		// Token: 0x04000039 RID: 57
		private uint topPos;

		// Token: 0x0400003A RID: 58
		private VMStack.LocallocNode localPool;

		// Token: 0x0200007D RID: 125
		private class LocallocNode
		{
			// Token: 0x060001AE RID: 430 RVA: 0x0000C918 File Offset: 0x0000AB18
			protected override void Finalize()
			{
				try
				{
					bool flag = this.Memory != IntPtr.Zero;
					if (flag)
					{
						Marshal.FreeHGlobal(this.Memory);
						this.Memory = IntPtr.Zero;
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x060001AF RID: 431 RVA: 0x0000C970 File Offset: 0x0000AB70
			public VMStack.LocallocNode Free()
			{
				bool flag = this.Memory != IntPtr.Zero;
				if (flag)
				{
					Marshal.FreeHGlobal(this.Memory);
					this.Memory = IntPtr.Zero;
				}
				return this.Next;
			}

			// Token: 0x040000DE RID: 222
			public uint GuardPos;

			// Token: 0x040000DF RID: 223
			public IntPtr Memory;

			// Token: 0x040000E0 RID: 224
			public VMStack.LocallocNode Next;
		}
	}
}
