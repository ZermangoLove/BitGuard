using System;

namespace KoiVM.VMIL
{
	// Token: 0x020000B0 RID: 176
	public interface IPostTransform
	{
		// Token: 0x060002A6 RID: 678
		void Initialize(ILPostTransformer tr);

		// Token: 0x060002A7 RID: 679
		void Transform(ILPostTransformer tr);
	}
}
