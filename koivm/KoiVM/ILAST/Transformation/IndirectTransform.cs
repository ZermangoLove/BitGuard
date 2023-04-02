using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000116 RID: 278
	public class IndirectTransform : ITransformationHandler
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00003568 File Offset: 0x00001768
		public void Transform(ILASTTransformer tr)
		{
			tr.Tree.TraverseTree<ModuleDef>(new Action<ILASTExpression, ModuleDef>(IndirectTransform.Transform), tr.Method.Module);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001A018 File Offset: 0x00018218
		private static void Transform(ILASTExpression expr, ModuleDef module)
		{
			Code ilcode = expr.ILCode;
			Code code = ilcode;
			switch (code)
			{
			case Code.Ldind_I1:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.SByte.ToTypeDefOrRef();
				expr.Arguments = new IILASTNode[] { expr.Clone() };
				expr.ILCode = Code.Conv_I1;
				break;
			case Code.Ldind_U1:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Byte.ToTypeDefOrRef();
				break;
			case Code.Ldind_I2:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Int16.ToTypeDefOrRef();
				expr.Arguments = new IILASTNode[] { expr.Clone() };
				expr.ILCode = Code.Conv_I2;
				break;
			case Code.Ldind_U2:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.UInt16.ToTypeDefOrRef();
				break;
			case Code.Ldind_I4:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Int32.ToTypeDefOrRef();
				break;
			case Code.Ldind_U4:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.UInt32.ToTypeDefOrRef();
				break;
			case Code.Ldind_I8:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.UInt64.ToTypeDefOrRef();
				break;
			case Code.Ldind_I:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.IntPtr.ToTypeDefOrRef();
				break;
			case Code.Ldind_R4:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Single.ToTypeDefOrRef();
				break;
			case Code.Ldind_R8:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Double.ToTypeDefOrRef();
				break;
			case Code.Ldind_Ref:
				expr.ILCode = Code.Ldobj;
				expr.Operand = module.CorLibTypes.Object.ToTypeDefOrRef();
				break;
			case Code.Stind_Ref:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.Object.ToTypeDefOrRef();
				break;
			case Code.Stind_I1:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.SByte.ToTypeDefOrRef();
				break;
			case Code.Stind_I2:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.Int16.ToTypeDefOrRef();
				break;
			case Code.Stind_I4:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.Int32.ToTypeDefOrRef();
				break;
			case Code.Stind_I8:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.UInt64.ToTypeDefOrRef();
				break;
			case Code.Stind_R4:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.Single.ToTypeDefOrRef();
				break;
			case Code.Stind_R8:
				expr.ILCode = Code.Stobj;
				expr.Operand = module.CorLibTypes.Double.ToTypeDefOrRef();
				break;
			default:
				if (code == Code.Stind_I)
				{
					expr.ILCode = Code.Stobj;
					expr.Operand = module.CorLibTypes.IntPtr.ToTypeDefOrRef();
				}
				break;
			}
		}
	}
}
