using System;

namespace KoiVM.ILAST
{
	// Token: 0x02000112 RID: 274
	public interface ITransformationHandler
	{
		// Token: 0x0600046D RID: 1133
		void Initialize(ILASTTransformer tr);

		// Token: 0x0600046E RID: 1134
		void Transform(ILASTTransformer tr);
	}
}
