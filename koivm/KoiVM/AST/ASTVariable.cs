using System;

namespace KoiVM.AST
{
	// Token: 0x0200012B RID: 299
	public class ASTVariable
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000037A5 File Offset: 0x000019A5
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x000037AD File Offset: 0x000019AD
		public ASTType Type { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000037B6 File Offset: 0x000019B6
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x000037BE File Offset: 0x000019BE
		public string Name { get; set; }

		// Token: 0x060004EE RID: 1262 RVA: 0x0001C750 File Offset: 0x0001A950
		public override string ToString()
		{
			return this.Name;
		}
	}
}
