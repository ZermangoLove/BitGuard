using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000074 RID: 116
	public class ShlHandler : ITranslationHandler
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000BB48 File Offset: 0x00009D48
		public Code ILCode
		{
			get
			{
				return Code.Shl;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000BB5C File Offset: 0x00009D5C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.SHL)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
