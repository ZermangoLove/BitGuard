using System;

namespace KoiVM.AST.ILAST
{
	// Token: 0x02000150 RID: 336
	public class ILASTVariable : ASTVariable, IILASTNode
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001D694 File Offset: 0x0001B894
		ASTType? IILASTNode.Type
		{
			get
			{
				return new ASTType?(base.Type);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00003E86 File Offset: 0x00002086
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00003E8E File Offset: 0x0000208E
		public ILASTVariableType VariableType { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00003E97 File Offset: 0x00002097
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00003E9F File Offset: 0x0000209F
		public object Annotation { get; set; }
	}
}
