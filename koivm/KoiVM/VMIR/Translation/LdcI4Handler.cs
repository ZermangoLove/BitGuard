using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000081 RID: 129
	public class LdcI4Handler : ITranslationHandler
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		public Code ILCode
		{
			get
			{
				return Code.Ldc_I4;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.FromI4((int)expr.Operand);
		}
	}
}
