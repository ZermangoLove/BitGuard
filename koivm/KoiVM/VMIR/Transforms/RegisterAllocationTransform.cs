using System;
using KoiVM.VMIR.RegAlloc;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000AB RID: 171
	public class RegisterAllocationTransform : ITransform
	{
		// Token: 0x06000292 RID: 658 RVA: 0x00002856 File Offset: 0x00000A56
		public void Initialize(IRTransformer tr)
		{
			this.allocator = new RegisterAllocator(tr);
			this.allocator.Initialize();
			tr.Annotations[RegisterAllocationTransform.RegAllocatorKey] = this.allocator;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00002888 File Offset: 0x00000A88
		public void Transform(IRTransformer tr)
		{
			this.allocator.Allocate(tr.Block);
		}

		// Token: 0x040000EC RID: 236
		private RegisterAllocator allocator;

		// Token: 0x040000ED RID: 237
		public static readonly object RegAllocatorKey = new object();
	}
}
