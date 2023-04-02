using System;

namespace KoiVM.AST.IR
{
	// Token: 0x02000135 RID: 309
	public class IRPointer : IIROperand
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00003960 File Offset: 0x00001B60
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00003968 File Offset: 0x00001B68
		public IRRegister Register { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00003971 File Offset: 0x00001B71
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00003979 File Offset: 0x00001B79
		public int Offset { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00003982 File Offset: 0x00001B82
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x0000398A File Offset: 0x00001B8A
		public IRVariable SourceVariable { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00003993 File Offset: 0x00001B93
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x0000399B File Offset: 0x00001B9B
		public ASTType Type { get; set; }

		// Token: 0x0600052F RID: 1327 RVA: 0x0001CAB8 File Offset: 0x0001ACB8
		public override string ToString()
		{
			string prefix = this.Type.ToString();
			string offsetStr = "";
			bool flag = this.Offset > 0;
			if (flag)
			{
				offsetStr = string.Format(" + {0:x}h", this.Offset);
			}
			else
			{
				bool flag2 = this.Offset < 0;
				if (flag2)
				{
					offsetStr = string.Format(" - {0:x}h", -this.Offset);
				}
			}
			return string.Format("{0}:[{1}{2}]", prefix, this.Register, offsetStr);
		}
	}
}
