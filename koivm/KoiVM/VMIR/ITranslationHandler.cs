using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR
{
	// Token: 0x02000028 RID: 40
	public interface ITranslationHandler
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000EE RID: 238
		Code ILCode { get; }

		// Token: 0x060000EF RID: 239
		IIROperand Translate(ILASTExpression expr, IRTranslator tr);
	}
}
