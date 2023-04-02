using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000113 RID: 275
	public class ArrayTransform : ITransformationHandler
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001963C File Offset: 0x0001783C
		public void Transform(ILASTTransformer tr)
		{
			ModuleDef module = tr.Method.Module;
			tr.Tree.TraverseTree<ModuleDef>(new Action<ILASTExpression, ModuleDef>(ArrayTransform.Transform), module);
			for (int i = 0; i < tr.Tree.Count; i++)
			{
				IILASTStatement st = tr.Tree[i];
				ILASTExpression expr = VariableInlining.GetExpression(st);
				bool flag = expr == null;
				if (!flag)
				{
					switch (expr.ILCode)
					{
					case Code.Stelem_I:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.IntPtr.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_I1:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.SByte.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_I2:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Int16.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_I4:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Int32.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_I8:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Int64.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_R4:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Single.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_R8:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Double.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem_Ref:
						ArrayTransform.TransformSTELEM(expr, module, module.CorLibTypes.Object.ToTypeDefOrRef(), tr.Tree, ref i);
						break;
					case Code.Stelem:
						ArrayTransform.TransformSTELEM(expr, module, (ITypeDefOrRef)expr.Operand, tr.Tree, ref i);
						break;
					}
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019838 File Offset: 0x00017A38
		private static void Transform(ILASTExpression expr, ModuleDef module)
		{
			switch (expr.ILCode)
			{
			case Code.Newarr:
			{
				expr.ILCode = Code.Newobj;
				ITypeDefOrRef array = new SZArraySig(((ITypeDefOrRef)expr.Operand).ToTypeSig(true)).ToTypeDefOrRef();
				MethodSig ctorSig = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Int32);
				MemberRefUser ctorRef = new MemberRefUser(module, ".ctor", ctorSig, array);
				expr.Operand = ctorRef;
				break;
			}
			case Code.Ldlen:
			{
				expr.ILCode = Code.Call;
				TypeRef array2 = module.CorLibTypes.GetTypeRef("System", "Array");
				MethodSig lenSig = MethodSig.CreateInstance(module.CorLibTypes.Int32);
				MemberRefUser methodRef = new MemberRefUser(module, "get_Length", lenSig, array2);
				expr.Operand = methodRef;
				break;
			}
			case Code.Ldelema:
			{
				expr.ILCode = Code.Call;
				TypeSig elemType = ((ITypeDefOrRef)expr.Operand).ToTypeSig(true);
				ITypeDefOrRef array3 = new SZArraySig(elemType).ToTypeDefOrRef();
				MethodSig addrSig = MethodSig.CreateInstance(new ByRefSig(elemType), module.CorLibTypes.Int32);
				MemberRefUser addrRef = new MemberRefUser(module, "Address", addrSig, array3);
				expr.Operand = addrRef;
				break;
			}
			case Code.Ldelem_I1:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.SByte.ToTypeDefOrRef());
				break;
			case Code.Ldelem_U1:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Byte.ToTypeDefOrRef());
				break;
			case Code.Ldelem_I2:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Int16.ToTypeDefOrRef());
				break;
			case Code.Ldelem_U2:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.UInt16.ToTypeDefOrRef());
				break;
			case Code.Ldelem_I4:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Int32.ToTypeDefOrRef());
				break;
			case Code.Ldelem_U4:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.UInt32.ToTypeDefOrRef());
				break;
			case Code.Ldelem_I8:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Int64.ToTypeDefOrRef());
				break;
			case Code.Ldelem_I:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.IntPtr.ToTypeDefOrRef());
				break;
			case Code.Ldelem_R4:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Single.ToTypeDefOrRef());
				break;
			case Code.Ldelem_R8:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Double.ToTypeDefOrRef());
				break;
			case Code.Ldelem_Ref:
				ArrayTransform.TransformLDELEM(expr, module, module.CorLibTypes.Object.ToTypeDefOrRef());
				break;
			case Code.Ldelem:
				ArrayTransform.TransformLDELEM(expr, module, (ITypeDefOrRef)expr.Operand);
				break;
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00019B28 File Offset: 0x00017D28
		private static void TransformLDELEM(ILASTExpression expr, ModuleDef module, ITypeDefOrRef type)
		{
			TypeRef array = module.CorLibTypes.GetTypeRef("System", "Array");
			MethodSig getValSig = MethodSig.CreateInstance(module.CorLibTypes.Object, module.CorLibTypes.Int32);
			MemberRefUser getValRef = new MemberRefUser(module, "GetValue", getValSig, array);
			ILASTExpression getValue = new ILASTExpression
			{
				ILCode = Code.Call,
				Operand = getValRef,
				Arguments = expr.Arguments
			};
			expr.ILCode = Code.Unbox_Any;
			expr.Operand = (type.IsValueType ? module.CorLibTypes.Object.ToTypeDefOrRef() : type);
			expr.Type = new ASTType?(TypeInference.ToASTType(type.ToTypeSig(true)));
			expr.Arguments = new IILASTNode[] { getValue };
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019BF8 File Offset: 0x00017DF8
		private static void TransformSTELEM(ILASTExpression expr, ModuleDef module, ITypeDefOrRef type, ILASTTree tree, ref int index)
		{
			TypeRef array = module.CorLibTypes.GetTypeRef("System", "Array");
			MethodSig setValSig = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Object, module.CorLibTypes.Int32);
			MemberRefUser setValRef = new MemberRefUser(module, "SetValue", setValSig, array);
			bool flag = expr.Arguments[1] is ILASTVariable;
			ILASTVariable tmpVar;
			if (flag)
			{
				tmpVar = (ILASTVariable)expr.Arguments[1];
			}
			else
			{
				tmpVar = new ILASTVariable
				{
					Name = string.Format("arr_{0:x4}_1", expr.CILInstr.Offset),
					VariableType = ILASTVariableType.StackVar
				};
				int num = index;
				index = num + 1;
				tree.Insert(num, new ILASTAssignment
				{
					Variable = tmpVar,
					Value = (ILASTExpression)expr.Arguments[1]
				});
			}
			bool flag2 = expr.Arguments[2] is ILASTVariable;
			ILASTVariable tmpVar2;
			if (flag2)
			{
				tmpVar2 = (ILASTVariable)expr.Arguments[2];
			}
			else
			{
				tmpVar2 = new ILASTVariable
				{
					Name = string.Format("arr_{0:x4}_2", expr.CILInstr.Offset),
					VariableType = ILASTVariableType.StackVar
				};
				int num = index;
				index = num + 1;
				tree.Insert(num, new ILASTAssignment
				{
					Variable = tmpVar2,
					Value = (ILASTExpression)expr.Arguments[2]
				});
			}
			bool isPrimitive = type.IsPrimitive;
			if (isPrimitive)
			{
				ILASTExpression ilastexpression = new ILASTExpression();
				ilastexpression.ILCode = Code.Box;
				ilastexpression.Operand = type;
				ILASTExpression ilastexpression2 = ilastexpression;
				IILASTNode[] array2 = new ILASTVariable[] { tmpVar2 };
				ilastexpression2.Arguments = array2;
				ILASTExpression elem = ilastexpression;
				expr.Arguments[2] = tmpVar;
				expr.Arguments[1] = elem;
			}
			else
			{
				expr.Arguments[2] = tmpVar;
				expr.Arguments[1] = tmpVar2;
			}
			expr.ILCode = Code.Call;
			expr.Operand = setValRef;
		}
	}
}
