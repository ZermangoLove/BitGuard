using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000119 RID: 281
	public class StringTransform : ITransformationHandler
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000035C6 File Offset: 0x000017C6
		public void Transform(ILASTTransformer tr)
		{
			tr.Tree.TraverseTree<ILASTTransformer>(new Action<ILASTExpression, ILASTTransformer>(StringTransform.Transform), tr);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001AD84 File Offset: 0x00018F84
		private static void Transform(ILASTExpression expr, ILASTTransformer tr)
		{
			bool flag = expr.ILCode != Code.Ldstr;
			if (!flag)
			{
				string operand = (string)expr.Operand;
				expr.ILCode = Code.Box;
				expr.Operand = tr.Method.Module.CorLibTypes.String.ToTypeDefOrRef();
				expr.Arguments = new IILASTNode[]
				{
					new ILASTExpression
					{
						ILCode = Code.Ldc_I4,
						Operand = (int)tr.VM.Data.GetId(operand),
						Arguments = new IILASTNode[0]
					}
				};
			}
		}
	}
}
