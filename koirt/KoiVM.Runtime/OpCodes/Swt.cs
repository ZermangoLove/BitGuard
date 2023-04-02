using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000052 RID: 82
	internal class Swt : IOpCode
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00007DF4 File Offset: 0x00005FF4
		public byte Code
		{
			get
			{
				return Constants.OP_SWT;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007E0C File Offset: 0x0000600C
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot tblSlot = ctx.Stack[sp];
			VMSlot valSlot = ctx.Stack[sp - 1U];
			sp -= 2U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			uint index = valSlot.U4;
			ushort len = *(UIntPtr)(tblSlot.U8 - 2UL);
			bool flag = index < (uint)len;
			if (flag)
			{
				VMSlot[] registers = ctx.Registers;
				byte reg_IP = Constants.REG_IP;
				registers[(int)reg_IP].U8 = registers[(int)reg_IP].U8 + (ulong)((long)(*((UIntPtr)tblSlot.U8 + (UIntPtr)((IntPtr)((ulong)index * 4UL)))));
			}
			state = ExecutionState.Next;
		}
	}
}
