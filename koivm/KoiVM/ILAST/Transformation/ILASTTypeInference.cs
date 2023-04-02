using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x02000118 RID: 280
	public class ILASTTypeInference : ITransformationHandler
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001A3D4 File Offset: 0x000185D4
		public void Transform(ILASTTransformer tr)
		{
			foreach (IILASTStatement st in tr.Tree)
			{
				bool flag = st is ILASTExpression;
				if (flag)
				{
					this.ProcessExpression((ILASTExpression)st);
				}
				else
				{
					bool flag2 = st is ILASTAssignment;
					if (flag2)
					{
						ILASTAssignment assignment = (ILASTAssignment)st;
						assignment.Variable.Type = this.ProcessExpression(assignment.Value).Value;
					}
					else
					{
						bool flag3 = st is ILASTPhi;
						if (flag3)
						{
							this.ProcessPhiNode((ILASTPhi)st);
						}
					}
				}
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000035AA File Offset: 0x000017AA
		private void ProcessPhiNode(ILASTPhi phi)
		{
			phi.Variable.Type = phi.SourceVariables[0].Type;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001A4A4 File Offset: 0x000186A4
		private ASTType? ProcessExpression(ILASTExpression expr)
		{
			foreach (IILASTNode arg in expr.Arguments)
			{
				bool flag = arg is ILASTExpression;
				if (flag)
				{
					ILASTExpression argExpr = (ILASTExpression)arg;
					argExpr.Type = new ASTType?(this.ProcessExpression(argExpr).Value);
				}
			}
			ASTType? exprType = ILASTTypeInference.InferType(expr);
			bool flag2 = exprType != null;
			if (flag2)
			{
				expr.Type = new ASTType?(exprType.Value);
			}
			return exprType;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001A534 File Offset: 0x00018734
		private static ASTType? InferType(ILASTExpression expr)
		{
			bool flag = expr.Type != null;
			ASTType? asttype;
			if (flag)
			{
				asttype = expr.Type;
			}
			else
			{
				OpCode opCode = expr.ILCode.ToOpCode();
				switch (opCode.StackBehaviourPush)
				{
				case StackBehaviour.Push1:
					return ILASTTypeInference.InferPush1(expr);
				case StackBehaviour.Push1_push1:
					Debug.Assert(expr.Arguments.Length == 1);
					return expr.Arguments[0].Type;
				case StackBehaviour.Pushi:
					return ILASTTypeInference.InferPushI(expr);
				case StackBehaviour.Pushi8:
					return ILASTTypeInference.InferPushI8(expr);
				case StackBehaviour.Pushr4:
					return ILASTTypeInference.InferPushR4(expr);
				case StackBehaviour.Pushr8:
					return ILASTTypeInference.InferPushR8(expr);
				case StackBehaviour.Pushref:
					return ILASTTypeInference.InferPushRef(expr);
				case StackBehaviour.Varpush:
					return ILASTTypeInference.InferVarPush(expr);
				}
				asttype = null;
			}
			return asttype;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001A61C File Offset: 0x0001881C
		private static ASTType? InferPush1(ILASTExpression expr)
		{
			Code ilcode = expr.ILCode;
			Code code = ilcode;
			if (code <= Code.Ldelem)
			{
				if (code <= Code.Ldfld)
				{
					switch (code)
					{
					case Code.Add:
					case Code.Sub:
					case Code.Mul:
					case Code.Div:
					case Code.Div_Un:
					case Code.Rem:
					case Code.Rem_Un:
						goto IL_105;
					case Code.And:
					case Code.Or:
					case Code.Xor:
						Debug.Assert(expr.Arguments.Length == 2);
						Debug.Assert(expr.Arguments[0].Type != null && expr.Arguments[1].Type != null);
						return new ASTType?(TypeInference.InferIntegerOp(expr.Arguments[0].Type.Value, expr.Arguments[1].Type.Value));
					case Code.Shl:
					case Code.Shr:
					case Code.Shr_Un:
						Debug.Assert(expr.Arguments.Length == 2);
						Debug.Assert(expr.Arguments[0].Type != null && expr.Arguments[1].Type != null);
						return new ASTType?(TypeInference.InferShiftOp(expr.Arguments[0].Type.Value, expr.Arguments[1].Type.Value));
					case Code.Neg:
					{
						Debug.Assert(expr.Arguments.Length == 1 && expr.Arguments[0].Type != null);
						ASTType? asttype = expr.Arguments[0].Type;
						ASTType asttype2 = ASTType.I4;
						bool flag;
						if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
						{
							asttype = expr.Arguments[0].Type;
							asttype2 = ASTType.I8;
							if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
							{
								asttype = expr.Arguments[0].Type;
								asttype2 = ASTType.R4;
								if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
								{
									asttype = expr.Arguments[0].Type;
									asttype2 = ASTType.R8;
									if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
									{
										asttype = expr.Arguments[0].Type;
										asttype2 = ASTType.Ptr;
										flag = !((asttype.GetValueOrDefault() == asttype2) & (asttype != null));
										goto IL_3B6;
									}
								}
							}
						}
						flag = false;
						IL_3B6:
						bool flag2 = flag;
						if (flag2)
						{
							throw new ArgumentException("Invalid Not Operand Types.");
						}
						return expr.Arguments[0].Type;
					}
					case Code.Not:
					{
						Debug.Assert(expr.Arguments.Length == 1 && expr.Arguments[0].Type != null);
						ASTType? asttype = expr.Arguments[0].Type;
						ASTType asttype2 = ASTType.I4;
						bool flag3;
						if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
						{
							asttype = expr.Arguments[0].Type;
							asttype2 = ASTType.I8;
							if (!((asttype.GetValueOrDefault() == asttype2) & (asttype != null)))
							{
								asttype = expr.Arguments[0].Type;
								asttype2 = ASTType.Ptr;
								flag3 = !((asttype.GetValueOrDefault() == asttype2) & (asttype != null));
								goto IL_2A4;
							}
						}
						flag3 = false;
						IL_2A4:
						bool flag4 = flag3;
						if (flag4)
						{
							throw new ArgumentException("Invalid Not Operand Types.");
						}
						return expr.Arguments[0].Type;
					}
					case Code.Conv_I1:
					case Code.Conv_I2:
					case Code.Conv_I4:
					case Code.Conv_I8:
					case Code.Conv_R4:
					case Code.Conv_R8:
					case Code.Conv_U4:
					case Code.Conv_U8:
					case Code.Callvirt:
					case Code.Cpobj:
						goto IL_4E0;
					case Code.Ldobj:
						goto IL_4A0;
					default:
						if (code != Code.Ldfld)
						{
							goto IL_4E0;
						}
						break;
					}
				}
				else if (code != Code.Ldsfld)
				{
					if (code != Code.Ldelem)
					{
						goto IL_4E0;
					}
					goto IL_4A0;
				}
				return new ASTType?(TypeInference.ToASTType(((IField)expr.Operand).FieldSig.Type));
			}
			if (code <= Code.Mkrefany)
			{
				if (code == Code.Unbox_Any)
				{
					goto IL_4A0;
				}
				if (code != Code.Mkrefany)
				{
					goto IL_4E0;
				}
				return new ASTType?(ASTType.O);
			}
			else if (code - Code.Add_Ovf > 5)
			{
				if (code == Code.Ldarg)
				{
					return new ASTType?(TypeInference.ToASTType(((Parameter)expr.Operand).Type));
				}
				if (code != Code.Ldloc)
				{
					goto IL_4E0;
				}
				return new ASTType?(TypeInference.ToASTType(((Local)expr.Operand).Type));
			}
			IL_105:
			Debug.Assert(expr.Arguments.Length == 2);
			Debug.Assert(expr.Arguments[0].Type != null && expr.Arguments[1].Type != null);
			return new ASTType?(TypeInference.InferBinaryOp(expr.Arguments[0].Type.Value, expr.Arguments[1].Type.Value));
			IL_4A0:
			return new ASTType?(TypeInference.ToASTType(((ITypeDefOrRef)expr.Operand).ToTypeSig(true)));
			IL_4E0:
			throw new NotSupportedException(expr.ILCode.ToString());
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001AB28 File Offset: 0x00018D28
		private static ASTType? InferPushI(ILASTExpression expr)
		{
			Code ilcode = expr.ILCode;
			Code code = ilcode;
			if (code <= Code.Refanyval)
			{
				if (code <= Code.Ldflda)
				{
					if (code <= Code.Isinst)
					{
						if (code != Code.Ldind_I)
						{
							if (code != Code.Isinst)
							{
								goto IL_12C;
							}
							goto IL_123;
						}
					}
					else
					{
						if (code == Code.Unbox)
						{
							goto IL_123;
						}
						if (code != Code.Ldflda)
						{
							goto IL_12C;
						}
						goto IL_11A;
					}
				}
				else if (code <= Code.Conv_Ovf_U_Un)
				{
					if (code == Code.Ldsflda)
					{
						goto IL_11A;
					}
					if (code - Code.Conv_Ovf_I_Un > 1)
					{
						goto IL_12C;
					}
				}
				else
				{
					if (code == Code.Ldelema)
					{
						goto IL_11A;
					}
					if (code != Code.Ldelem_I)
					{
						if (code != Code.Refanyval)
						{
							goto IL_12C;
						}
						goto IL_123;
					}
				}
			}
			else if (code <= Code.Arglist)
			{
				if (code <= Code.Conv_Ovf_U)
				{
					if (code == Code.Ldtoken)
					{
						goto IL_123;
					}
					if (code - Code.Conv_I > 2)
					{
						goto IL_12C;
					}
				}
				else if (code != Code.Conv_U)
				{
					if (code != Code.Arglist)
					{
						goto IL_12C;
					}
					goto IL_123;
				}
			}
			else if (code <= Code.Ldarga)
			{
				if (code - Code.Ldftn > 1)
				{
					if (code != Code.Ldarga)
					{
						goto IL_12C;
					}
					goto IL_11A;
				}
			}
			else
			{
				if (code == Code.Ldloca)
				{
					goto IL_11A;
				}
				if (code != Code.Localloc)
				{
					if (code != Code.Refanytype)
					{
						goto IL_12C;
					}
					goto IL_123;
				}
			}
			return new ASTType?(ASTType.Ptr);
			IL_11A:
			return new ASTType?(ASTType.ByRef);
			IL_123:
			return new ASTType?(ASTType.O);
			IL_12C:
			return new ASTType?(ASTType.I4);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001AC6C File Offset: 0x00018E6C
		private static ASTType? InferPushI8(ILASTExpression expr)
		{
			return new ASTType?(ASTType.I8);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001AC84 File Offset: 0x00018E84
		private static ASTType? InferPushR4(ILASTExpression expr)
		{
			return new ASTType?(ASTType.R4);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001AC9C File Offset: 0x00018E9C
		private static ASTType? InferPushR8(ILASTExpression expr)
		{
			return new ASTType?(ASTType.R8);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001ACB4 File Offset: 0x00018EB4
		private static ASTType? InferPushRef(ILASTExpression expr)
		{
			return new ASTType?(ASTType.O);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001ACCC File Offset: 0x00018ECC
		private static ASTType? InferVarPush(ILASTExpression expr)
		{
			IMethod method = (IMethod)expr.Operand;
			bool flag = method.MethodSig.RetType.ElementType == ElementType.Void;
			ASTType? asttype;
			if (flag)
			{
				asttype = null;
			}
			else
			{
				GenericArguments genArgs = new GenericArguments();
				bool flag2 = method is MethodSpec;
				if (flag2)
				{
					genArgs.PushMethodArgs(((MethodSpec)method).GenericInstMethodSig.GenericArguments);
				}
				bool flag3 = method.DeclaringType.TryGetGenericInstSig() != null;
				if (flag3)
				{
					genArgs.PushTypeArgs(method.DeclaringType.TryGetGenericInstSig().GenericArguments);
				}
				asttype = new ASTType?(TypeInference.ToASTType(genArgs.ResolveType(method.MethodSig.RetType)));
			}
			return asttype;
		}
	}
}
