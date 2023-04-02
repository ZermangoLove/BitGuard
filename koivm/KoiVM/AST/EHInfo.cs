using System;
using dnlib.DotNet.Emit;

namespace KoiVM.AST
{
	// Token: 0x02000130 RID: 304
	public class EHInfo : InstrAnnotation
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		public EHInfo(ExceptionHandler eh)
			: base("EH_" + eh.GetHashCode().ToString())
		{
			this.ExceptionHandler = eh;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x000038A1 File Offset: 0x00001AA1
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x000038A9 File Offset: 0x00001AA9
		public ExceptionHandler ExceptionHandler { get; set; }
	}
}
