using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000075 RID: 117
	public class ShrHandler : ITranslationHandler
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public Code ILCode
		{
			get
			{
				return Code.Shr;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000BC10 File Offset: 0x00009E10
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
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
