using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000073 RID: 115
	public class NotHandler : ITranslationHandler
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000BAAC File Offset: 0x00009CAC
		public Code ILCode
		{
			get
			{
				return Code.Not;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__NOT)
			{
				Operand1 = ret
			});
			return ret;
		}
	}
}
