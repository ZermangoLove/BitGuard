using System;
using KoiVM.RT;

namespace KoiVM.AST.IR
{
	// Token: 0x02000133 RID: 307
	public class IRDataTarget : IIROperand
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x000038F8 File Offset: 0x00001AF8
		public IRDataTarget(BinaryChunk target)
		{
			this.Target = target;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000390A File Offset: 0x00001B0A
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00003912 File Offset: 0x00001B12
		public BinaryChunk Target { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000391B File Offset: 0x00001B1B
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00003923 File Offset: 0x00001B23
		public string Name { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001CA14 File Offset: 0x0001AC14
		public ASTType Type
		{
			get
			{
				return ASTType.Ptr;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001CA80 File Offset: 0x0001AC80
		public override string ToString()
		{
			return this.Name;
		}
	}
}
