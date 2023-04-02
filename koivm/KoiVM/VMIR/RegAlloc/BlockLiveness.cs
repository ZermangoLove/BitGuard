using System;
using System.Collections.Generic;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.RegAlloc
{
	// Token: 0x0200002B RID: 43
	public class BlockLiveness
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00002655 File Offset: 0x00000855
		private BlockLiveness(HashSet<IRVariable> inLive, HashSet<IRVariable> outLive)
		{
			this.InLive = inLive;
			this.OutLive = outLive;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000266F File Offset: 0x0000086F
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00002677 File Offset: 0x00000877
		public HashSet<IRVariable> InLive { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00002688 File Offset: 0x00000888
		public HashSet<IRVariable> OutLive { get; private set; }

		// Token: 0x060000F6 RID: 246 RVA: 0x00006E70 File Offset: 0x00005070
		internal static BlockLiveness Empty()
		{
			return new BlockLiveness(new HashSet<IRVariable>(), new HashSet<IRVariable>());
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006E94 File Offset: 0x00005094
		internal BlockLiveness Clone()
		{
			return new BlockLiveness(new HashSet<IRVariable>(this.InLive), new HashSet<IRVariable>(this.OutLive));
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006EC4 File Offset: 0x000050C4
		public override string ToString()
		{
			return string.Format("In=[{0}], Out=[{1}]", string.Join<IRVariable>(", ", this.InLive), string.Join<IRVariable>(", ", this.OutLive));
		}
	}
}
