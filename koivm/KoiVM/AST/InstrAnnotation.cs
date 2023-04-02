using System;

namespace KoiVM.AST
{
	// Token: 0x0200012D RID: 301
	public class InstrAnnotation
	{
		// Token: 0x060004F4 RID: 1268 RVA: 0x000037C7 File Offset: 0x000019C7
		public InstrAnnotation(string name)
		{
			this.Name = name;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000037D9 File Offset: 0x000019D9
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x000037E1 File Offset: 0x000019E1
		public string Name { get; private set; }

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001C990 File Offset: 0x0001AB90
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000225 RID: 549
		public static readonly InstrAnnotation JUMP = new InstrAnnotation("JUMP");
	}
}
