using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000085 RID: 133
	public class LdnullHandler : ITranslationHandler
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		public Code ILCode
		{
			get
			{
				return Code.Ldnull;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.Null();
		}
	}
}
