using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000086 RID: 134
	public class LdstrHandler : ITranslationHandler
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000C8DC File Offset: 0x0000AADC
		public Code ILCode
		{
			get
			{
				return Code.Ldstr;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			return IRConstant.FromString((string)expr.Operand);
		}
	}
}
