using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006F RID: 111
	public class PopHandler : ITranslationHandler
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000B848 File Offset: 0x00009A48
		public Code ILCode
		{
			get
			{
				return Code.Pop;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B85C File Offset: 0x00009A5C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			tr.Translate(expr.Arguments[0]);
			return null;
		}
	}
}
