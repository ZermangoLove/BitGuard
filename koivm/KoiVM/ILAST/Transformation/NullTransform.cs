using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000117 RID: 279
	public class NullTransform : ITransformationHandler
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000358E File Offset: 0x0000178E
		public void Transform(ILASTTransformer tr)
		{
			tr.Tree.TraverseTree<ILASTTransformer>(new Action<ILASTExpression, ILASTTransformer>(NullTransform.Transform), tr);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001A39C File Offset: 0x0001859C
		private static void Transform(ILASTExpression expr, ILASTTransformer tr)
		{
			bool flag = expr.ILCode != Code.Ldnull;
			if (!flag)
			{
				expr.ILCode = Code.Ldc_I4;
				expr.Operand = 0;
			}
		}
	}
}
