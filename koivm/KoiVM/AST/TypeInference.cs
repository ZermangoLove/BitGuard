using System;
using dnlib.DotNet;

namespace KoiVM.AST
{
	// Token: 0x0200012C RID: 300
	public static class TypeInference
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x0001C768 File Offset: 0x0001A968
		public static ASTType ToASTType(TypeSig type)
		{
			switch (type.ElementType)
			{
			case ElementType.Boolean:
			case ElementType.Char:
			case ElementType.I1:
			case ElementType.U1:
			case ElementType.I2:
			case ElementType.U2:
			case ElementType.I4:
			case ElementType.U4:
				return ASTType.I4;
			case ElementType.I8:
			case ElementType.U8:
				return ASTType.I8;
			case ElementType.R4:
				return ASTType.R4;
			case ElementType.R8:
				return ASTType.R8;
			case ElementType.Ptr:
			case ElementType.I:
			case ElementType.U:
			case ElementType.FnPtr:
				return ASTType.Ptr;
			case ElementType.ByRef:
				return ASTType.ByRef;
			case ElementType.ValueType:
			{
				TypeDef typeDef = type.ScopeType.ResolveTypeDef();
				bool flag = typeDef != null && typeDef.IsEnum;
				if (flag)
				{
					return TypeInference.ToASTType(typeDef.GetEnumUnderlyingType());
				}
				return ASTType.O;
			}
			}
			return ASTType.O;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001C840 File Offset: 0x0001AA40
		public static ASTType InferBinaryOp(ASTType a, ASTType b)
		{
			bool flag = a == b && (a == ASTType.I4 || a == ASTType.I8 || a == ASTType.R4 || a == ASTType.R8);
			ASTType asttype;
			if (flag)
			{
				asttype = a;
			}
			else
			{
				bool flag2 = (a == ASTType.Ptr && (b == ASTType.I4 || b == ASTType.I8 || b == ASTType.Ptr)) || (b == ASTType.Ptr && (a == ASTType.I4 || b == ASTType.I4 || a == ASTType.Ptr));
				if (flag2)
				{
					asttype = ASTType.Ptr;
				}
				else
				{
					bool flag3 = (a == ASTType.ByRef && (b == ASTType.I4 || b == ASTType.Ptr)) || (b == ASTType.ByRef && (a == ASTType.I4 || a == ASTType.Ptr));
					if (flag3)
					{
						asttype = ASTType.ByRef;
					}
					else
					{
						bool flag4 = a == ASTType.ByRef && b == ASTType.ByRef;
						if (!flag4)
						{
							throw new ArgumentException("Invalid Binary Op Operand Types.");
						}
						asttype = ASTType.Ptr;
					}
				}
			}
			return asttype;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
		public static ASTType InferIntegerOp(ASTType a, ASTType b)
		{
			bool flag = a == b && (a == ASTType.I4 || a == ASTType.I8 || a == ASTType.R4 || a == ASTType.R8);
			ASTType asttype;
			if (flag)
			{
				asttype = a;
			}
			else
			{
				bool flag2 = (a == ASTType.Ptr && (b == ASTType.I4 || b == ASTType.I8 || b == ASTType.Ptr)) || (b == ASTType.Ptr && (a == ASTType.I4 || b == ASTType.I8 || a == ASTType.Ptr));
				if (!flag2)
				{
					throw new ArgumentException("Invalid Integer Op Operand Types.");
				}
				asttype = ASTType.Ptr;
			}
			return asttype;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001C954 File Offset: 0x0001AB54
		public static ASTType InferShiftOp(ASTType a, ASTType b)
		{
			bool flag = (b == ASTType.Ptr || b == ASTType.I4) && (a == ASTType.I4 || b == ASTType.I4 || a == ASTType.Ptr);
			if (flag)
			{
				return a;
			}
			throw new ArgumentException("Invalid Shift Op Operand Types.");
		}
	}
}
