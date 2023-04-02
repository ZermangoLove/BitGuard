using System;

namespace KoiVM.AST
{
	// Token: 0x02000128 RID: 296
	public abstract class ASTExpression : ASTNode
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000378B File Offset: 0x0000198B
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00003793 File Offset: 0x00001993
		public ASTType? Type { get; set; }
	}
}
