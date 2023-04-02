using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006E RID: 110
	public class CkfiniteHandler : ITranslationHandler
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000B788 File Offset: 0x00009988
		public Code ILCode
		{
			get
			{
				return Code.Ckfinite;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			int ecallId = tr.VM.Runtime.VMCall.CKFINITE;
			bool flag = value.Type == ASTType.R4;
			if (flag)
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
				{
					Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
				});
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), value));
			return value;
		}
	}
}
