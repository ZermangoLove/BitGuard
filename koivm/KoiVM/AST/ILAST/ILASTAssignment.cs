using System;

namespace KoiVM.AST.ILAST
{
	// Token: 0x0200014B RID: 331
	public class ILASTAssignment : ASTNode, IILASTStatement
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00003DB0 File Offset: 0x00001FB0
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public ILASTVariable Variable { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00003DC1 File Offset: 0x00001FC1
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00003DC9 File Offset: 0x00001FC9
		public ILASTExpression Value { get; set; }

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001D260 File Offset: 0x0001B460
		public override string ToString()
		{
			return string.Format("{0} = {1}", this.Variable, this.Value);
		}
	}
}
