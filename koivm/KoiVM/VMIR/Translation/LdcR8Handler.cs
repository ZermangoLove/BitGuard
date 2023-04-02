using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000084 RID: 132
	public class LdcR8Handler : ITranslationHandler
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000C878 File Offset: 0x0000AA78
		public Code ILCode
		{
			get
			{
				return Code.Ldc_R8;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000C88C File Offset: 0x0000AA8C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.FromR8((double)expr.Operand);
		}
	}
}
