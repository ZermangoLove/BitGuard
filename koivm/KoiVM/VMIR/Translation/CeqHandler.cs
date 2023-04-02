using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200007C RID: 124
	public class CeqHandler : ITranslationHandler
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000C0C4 File Offset: 0x0000A2C4
		public Code ILCode
		{
			get
			{
				return Code.Ceq;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C0DC File Offset: 0x0000A2DC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.I4);
			TranslationHelpers.EmitCompareEq(tr, expr.Arguments[0].Type.Value, tr.Translate(expr.Arguments[0]), tr.Translate(expr.Arguments[1]));
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			return ret;
		}
	}
}
