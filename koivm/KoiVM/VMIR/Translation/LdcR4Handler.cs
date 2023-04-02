using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000083 RID: 131
	public class LdcR4Handler : ITranslationHandler
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000C840 File Offset: 0x0000AA40
		public Code ILCode
		{
			get
			{
				return Code.Ldc_R4;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C854 File Offset: 0x0000AA54
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.FromR4((float)expr.Operand);
		}
	}
}
