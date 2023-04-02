using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000037 RID: 55
	public class UnboxHandler : ITranslationHandler
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00007D0C File Offset: 0x00005F0C
		public Code ILCode
		{
			get
			{
				return Code.Unbox;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007D20 File Offset: 0x00005F20
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			TypeSig targetType = ((ITypeDefOrRef)expr.Operand).ToTypeSig(true);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int typeId = (int)(tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand) | 2147483648U);
			int ecallId = tr.VM.Runtime.VMCall.UNBOX;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
