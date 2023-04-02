using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000114 RID: 276
	public class BranchTransform : ITransformationHandler
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00003536 File Offset: 0x00001736
		public void Transform(ILASTTransformer tr)
		{
			tr.Tree.TraverseTree<ModuleDef>(new Action<ILASTExpression, ModuleDef>(BranchTransform.Transform), tr.Method.Module);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00019DF8 File Offset: 0x00017FF8
		private static void Transform(ILASTExpression expr, ModuleDef module)
		{
			Code ilcode = expr.ILCode;
			Code code = ilcode;
			if (code - Code.Beq <= 9)
			{
				Debug.Assert(expr.Arguments.Length == 2);
				Tuple<Code, Code, Code> mapInfo = BranchTransform.transformMap[expr.ILCode];
				Code compCode = (expr.Arguments.Any((IILASTNode arg) => arg.Type.Value == ASTType.R4 || arg.Type.Value == ASTType.R8) ? mapInfo.Item2 : mapInfo.Item1);
				expr.ILCode = mapInfo.Item3;
				expr.Arguments = new IILASTNode[]
				{
					new ILASTExpression
					{
						ILCode = compCode,
						Arguments = expr.Arguments,
						Type = new ASTType?(ASTType.I4)
					}
				};
			}
		}

		// Token: 0x040001F7 RID: 503
		private static readonly Dictionary<Code, Tuple<Code, Code, Code>> transformMap = new Dictionary<Code, Tuple<Code, Code, Code>>
		{
			{
				Code.Beq,
				Tuple.Create<Code, Code, Code>(Code.Ceq, Code.Ceq, Code.Brtrue)
			},
			{
				Code.Bne_Un,
				Tuple.Create<Code, Code, Code>(Code.Ceq, Code.Ceq, Code.Brfalse)
			},
			{
				Code.Bge,
				Tuple.Create<Code, Code, Code>(Code.Clt, Code.Clt_Un, Code.Brfalse)
			},
			{
				Code.Bge_Un,
				Tuple.Create<Code, Code, Code>(Code.Clt_Un, Code.Clt, Code.Brfalse)
			},
			{
				Code.Ble,
				Tuple.Create<Code, Code, Code>(Code.Cgt, Code.Cgt_Un, Code.Brfalse)
			},
			{
				Code.Ble_Un,
				Tuple.Create<Code, Code, Code>(Code.Cgt_Un, Code.Cgt, Code.Brfalse)
			},
			{
				Code.Bgt,
				Tuple.Create<Code, Code, Code>(Code.Cgt, Code.Cgt, Code.Brtrue)
			},
			{
				Code.Bgt_Un,
				Tuple.Create<Code, Code, Code>(Code.Cgt_Un, Code.Cgt_Un, Code.Brtrue)
			},
			{
				Code.Blt,
				Tuple.Create<Code, Code, Code>(Code.Clt, Code.Clt, Code.Brtrue)
			},
			{
				Code.Blt_Un,
				Tuple.Create<Code, Code, Code>(Code.Clt_Un, Code.Clt_Un, Code.Brtrue)
			}
		};
	}
}
