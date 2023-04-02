using System;
using KoiVM.CFG;

namespace KoiVM.AST.IR
{
	// Token: 0x02000132 RID: 306
	public class IRJumpTable : IIROperand
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x000038D5 File Offset: 0x00001AD5
		public IRJumpTable(IBasicBlock[] targets)
		{
			this.Targets = targets;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000038E7 File Offset: 0x00001AE7
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x000038EF File Offset: 0x00001AEF
		public IBasicBlock[] Targets { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001CA14 File Offset: 0x0001AC14
		public ASTType Type
		{
			get
			{
				return ASTType.Ptr;
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001CA54 File Offset: 0x0001AC54
		public override string ToString()
		{
			return string.Format("[..{0}..]", this.Targets.Length);
		}
	}
}
