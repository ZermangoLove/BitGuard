using System;
using System.Collections.Generic;
using KoiVM.Runtime.Dynamic;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000066 RID: 102
	internal class VMContext
	{
		// Token: 0x06000142 RID: 322 RVA: 0x000089B0 File Offset: 0x00006BB0
		public VMContext(VMInstance inst)
		{
			this.Instance = inst;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000089F0 File Offset: 0x00006BF0
		public unsafe byte ReadByte()
		{
			uint key = this.Registers[(int)Constants.REG_K1].U4;
			VMSlot[] registers = this.Registers;
			byte reg_IP = Constants.REG_IP;
			ulong u = registers[(int)reg_IP].U8;
			registers[(int)reg_IP].U8 = u + 1UL;
			byte* ip = u;
			byte b = (byte)((uint)(*ip) ^ key);
			key = key * 7U + (uint)b;
			this.Registers[(int)Constants.REG_K1].U4 = key;
			return b;
		}

		// Token: 0x04000027 RID: 39
		private const int NumRegisters = 16;

		// Token: 0x04000028 RID: 40
		public readonly VMSlot[] Registers = new VMSlot[16];

		// Token: 0x04000029 RID: 41
		public readonly VMStack Stack = new VMStack();

		// Token: 0x0400002A RID: 42
		public readonly VMInstance Instance;

		// Token: 0x0400002B RID: 43
		public readonly List<EHFrame> EHStack = new List<EHFrame>();

		// Token: 0x0400002C RID: 44
		public readonly List<EHState> EHStates = new List<EHState>();
	}
}
