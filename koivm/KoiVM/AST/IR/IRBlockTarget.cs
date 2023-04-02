using System;
using KoiVM.CFG;

namespace KoiVM.AST.IR
{
	// Token: 0x02000131 RID: 305
	public class IRBlockTarget : IIROperand
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x000038B2 File Offset: 0x00001AB2
		public IRBlockTarget(IBasicBlock target)
		{
			this.Target = target;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x000038C4 File Offset: 0x00001AC4
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x000038CC File Offset: 0x00001ACC
		public IBasicBlock Target { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001CA14 File Offset: 0x0001AC14
		public ASTType Type
		{
			get
			{
				return ASTType.Ptr;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001CA28 File Offset: 0x0001AC28
		public override string ToString()
		{
			return string.Format("Block_{0:x2}", this.Target.Id);
		}
	}
}
