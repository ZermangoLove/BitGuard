using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200007E RID: 126
	public class CgtUnHandler : ITranslationHandler
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000C398 File Offset: 0x0000A598
		public Code ILCode
		{
			get
			{
				return Code.Cgt_Un;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			tr.Instructions.Add(new IRInstruction(IROpCode.CMP)
			{
				Operand1 = tr.Translate(expr.Arguments[0]),
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4((1 << tr.Arch.Flags.CARRY) | (1 << tr.Arch.Flags.ZERO))
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__OR)
			{
				Operand1 = ret,
				Operand2 = ret
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			return ret;
		}
	}
}
