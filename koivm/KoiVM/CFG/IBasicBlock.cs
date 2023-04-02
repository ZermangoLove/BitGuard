using System;
using System.Collections.Generic;

namespace KoiVM.CFG
{
	// Token: 0x02000122 RID: 290
	public interface IBasicBlock
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004C1 RID: 1217
		int Id { get; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004C2 RID: 1218
		object Content { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004C3 RID: 1219
		// (set) Token: 0x060004C4 RID: 1220
		BlockFlags Flags { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004C5 RID: 1221
		IEnumerable<IBasicBlock> Sources { get; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004C6 RID: 1222
		IEnumerable<IBasicBlock> Targets { get; }
	}
}
