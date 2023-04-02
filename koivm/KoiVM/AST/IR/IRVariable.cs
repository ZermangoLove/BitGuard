using System;
using dnlib.DotNet;

namespace KoiVM.AST.IR
{
	// Token: 0x0200013B RID: 315
	public class IRVariable : ASTVariable, IIROperand
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00003B29 File Offset: 0x00001D29
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x00003B31 File Offset: 0x00001D31
		public IRVariableType VariableType { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00003B3A File Offset: 0x00001D3A
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x00003B42 File Offset: 0x00001D42
		public TypeSig RawType { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00003B4B File Offset: 0x00001D4B
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x00003B53 File Offset: 0x00001D53
		public int Id { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00003B5C File Offset: 0x00001D5C
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00003B64 File Offset: 0x00001D64
		public object Annotation { get; set; }

		// Token: 0x0600055F RID: 1375 RVA: 0x0001CE28 File Offset: 0x0001B028
		public override string ToString()
		{
			return string.Format("{0}:{1}", base.Name, base.Type);
		}
	}
}
