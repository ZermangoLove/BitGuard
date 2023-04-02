using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000091 RID: 145
	public class AddHandler : ITranslationHandler
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000D184 File Offset: 0x0000B384
		public Code ILCode
		{
			get
			{
				return Code.Add;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000D198 File Offset: 0x0000B398
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
