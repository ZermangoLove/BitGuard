using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009B RID: 155
	public class DivUnHandler : ITranslationHandler
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000DF7C File Offset: 0x0000C17C
		public Code ILCode
		{
			get
			{
				return Code.Div_Un;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000DF90 File Offset: 0x0000C190
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.DIV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
