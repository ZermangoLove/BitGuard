using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000082 RID: 130
	public class LdcI8Handler : ITranslationHandler
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000C808 File Offset: 0x0000AA08
		public Code ILCode
		{
			get
			{
				return Code.Ldc_I8;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C81C File Offset: 0x0000AA1C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.FromI8((long)expr.Operand);
		}
	}
}
