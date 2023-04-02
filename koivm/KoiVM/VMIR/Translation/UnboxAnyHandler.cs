using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000036 RID: 54
	public class UnboxAnyHandler : ITranslationHandler
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00007BDC File Offset: 0x00005DDC
		public Code ILCode
		{
			get
			{
				return Code.Unbox_Any;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			TypeSig targetType = ((ITypeDefOrRef)expr.Operand).ToTypeSig(true);
			bool flag = !targetType.GetElementType().IsPrimitive() && targetType.ElementType != ElementType.Object && !targetType.ToTypeDefOrRef().ResolveTypeDefThrow().IsEnum;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
				int typeId = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
				int ecallId = tr.VM.Runtime.VMCall.UNBOX;
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
				tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
