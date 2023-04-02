using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009A RID: 154
	public class DivHandler : ITranslationHandler
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
		public Code ILCode
		{
			get
			{
				return Code.Div;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
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
