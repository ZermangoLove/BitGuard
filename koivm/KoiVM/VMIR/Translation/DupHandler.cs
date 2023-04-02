using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006B RID: 107
	public class DupHandler : ITranslationHandler
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000B670 File Offset: 0x00009870
		public Code ILCode
		{
			get
			{
				return Code.Dup;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B684 File Offset: 0x00009884
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			return ret;
		}
	}
}
