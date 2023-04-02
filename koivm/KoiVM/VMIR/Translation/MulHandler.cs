using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000097 RID: 151
	public class MulHandler : ITranslationHandler
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000DB60 File Offset: 0x0000BD60
		public Code ILCode
		{
			get
			{
				return Code.Mul;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.MUL)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
