using System;

namespace KoiVM.VMIL
{
	// Token: 0x020000B9 RID: 185
	public interface ITransform
	{
		// Token: 0x060002D8 RID: 728
		void Initialize(ILTransformer tr);

		// Token: 0x060002D9 RID: 729
		void Transform(ILTransformer tr);
	}
}
