using System;
using System.Collections.Generic;
using KoiVM.CFG;

namespace KoiVM.VM
{
	// Token: 0x0200000E RID: 14
	public class VMMethodInfo
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000229D File Offset: 0x0000049D
		public VMMethodInfo()
		{
			this.BlockKeys = new Dictionary<IBasicBlock, VMBlockKey>();
			this.UsedRegister = new HashSet<VMRegisters>();
		}

		// Token: 0x04000022 RID: 34
		public ScopeBlock RootScope;

		// Token: 0x04000023 RID: 35
		public readonly Dictionary<IBasicBlock, VMBlockKey> BlockKeys;

		// Token: 0x04000024 RID: 36
		public readonly HashSet<VMRegisters> UsedRegister;

		// Token: 0x04000025 RID: 37
		public byte EntryKey;

		// Token: 0x04000026 RID: 38
		public byte ExitKey;
	}
}
