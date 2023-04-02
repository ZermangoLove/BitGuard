using System;

namespace KoiVM.VMIR
{
	// Token: 0x02000027 RID: 39
	public interface ITransform
	{
		// Token: 0x060000EC RID: 236
		void Initialize(IRTransformer tr);

		// Token: 0x060000ED RID: 237
		void Transform(IRTransformer tr);
	}
}
