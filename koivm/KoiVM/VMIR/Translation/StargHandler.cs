using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000069 RID: 105
	public class StargHandler : ITranslationHandler
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000B58C File Offset: 0x0000978C
		public Code ILCode
		{
			get
			{
				return Code.Starg;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B5A4 File Offset: 0x000097A4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = tr.Context.ResolveParameter((Parameter)expr.Operand),
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			return null;
		}
	}
}
