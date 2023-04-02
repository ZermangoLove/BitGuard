using System;

namespace KoiVM.AST.IR
{
	// Token: 0x02000134 RID: 308
	public class IRMetaTarget : IIROperand
	{
		// Token: 0x06000520 RID: 1312 RVA: 0x0000392C File Offset: 0x00001B2C
		public IRMetaTarget(object mdItem)
		{
			this.MetadataItem = mdItem;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000393E File Offset: 0x00001B3E
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00003946 File Offset: 0x00001B46
		public object MetadataItem { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000394F File Offset: 0x00001B4F
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x00003957 File Offset: 0x00001B57
		public bool LateResolve { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001CA14 File Offset: 0x0001AC14
		public ASTType Type
		{
			get
			{
				return ASTType.Ptr;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001CA98 File Offset: 0x0001AC98
		public override string ToString()
		{
			return this.MetadataItem.ToString();
		}
	}
}
