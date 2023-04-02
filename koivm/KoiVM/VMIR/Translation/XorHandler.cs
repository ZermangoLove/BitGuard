using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000072 RID: 114
	public class XorHandler : ITranslationHandler
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public Code ILCode
		{
			get
			{
				return Code.Xor;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000BA0C File Offset: 0x00009C0C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__XOR)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
