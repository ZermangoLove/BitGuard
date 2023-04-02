using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000090 RID: 144
	public class CalliHandler : ITranslationHandler
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000D170 File Offset: 0x0000B370
		public Code ILCode
		{
			get
			{
				return Code.Calli;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000274D File Offset: 0x0000094D
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			throw new NotSupportedException();
		}
	}
}
