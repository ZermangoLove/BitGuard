using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000058 RID: 88
	public class ConvOvfU8UnHandler : ITranslationHandler
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000A904 File Offset: 0x00008B04
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_U8_Un;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000A91C File Offset: 0x00008B1C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			return tr.Translate(expr.Arguments[0]);
		}
	}
}
