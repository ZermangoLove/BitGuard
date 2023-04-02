using System;
using dnlib.DotNet;

namespace KoiVM.AST
{
	// Token: 0x0200012F RID: 303
	public class PointerInfo : InstrAnnotation
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x0000387D File Offset: 0x00001A7D
		public PointerInfo(string name, ITypeDefOrRef ptrType)
			: base(name)
		{
			this.PointerType = ptrType;
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00003890 File Offset: 0x00001A90
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x00003898 File Offset: 0x00001A98
		public ITypeDefOrRef PointerType { get; set; }
	}
}
