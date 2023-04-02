using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000076 RID: 118
	public class ShrUnHandler : ITranslationHandler
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		public Code ILCode
		{
			get
			{
				return Code.Shr_Un;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000BCC4 File Offset: 0x00009EC4
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
			tr.Instructions.Add(new IRInstruction(IROpCode.SHR)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
