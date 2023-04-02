using System;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000065 RID: 101
	internal struct TypedRefPtr
	{
		// Token: 0x06000140 RID: 320 RVA: 0x00008974 File Offset: 0x00006B74
		public unsafe static implicit operator TypedRefPtr(void* ptr)
		{
			return new TypedRefPtr
			{
				ptr = ptr
			};
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008998 File Offset: 0x00006B98
		public unsafe static implicit operator void*(TypedRefPtr ptr)
		{
			return ptr.ptr;
		}

		// Token: 0x04000026 RID: 38
		public unsafe void* ptr;
	}
}
