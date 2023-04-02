using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000035 RID: 53
	public class BoxHandler : ITranslationHandler
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007A94 File Offset: 0x00005C94
		public Code ILCode
		{
			get
			{
				return Code.Box;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007AAC File Offset: 0x00005CAC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			TypeSig targetType = ((ITypeDefOrRef)expr.Operand).ToTypeSig(true);
			TypeDef boxType = ((ITypeDefOrRef)expr.Operand).ResolveTypeDef();
			bool flag = !targetType.GetElementType().IsPrimitive() && (boxType == null || !boxType.IsEnum);
			if (flag)
			{
				bool flag2 = targetType.ElementType != ElementType.String;
				if (flag2)
				{
					return value;
				}
			}
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int typeId = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.BOX;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
