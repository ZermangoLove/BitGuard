using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000057 RID: 87
	internal class PushRDword : IOpCode
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000080E8 File Offset: 0x000062E8
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHR_DWORD;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008100 File Offset: 0x00006300
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			byte regId = ctx.ReadByte();
			VMSlot slot = ctx.Registers[(int)regId];
			bool flag = regId == Constants.REG_SP || regId == Constants.REG_BP;
			if (flag)
			{
				ctx.Stack[sp] = new VMSlot
				{
					O = new StackRef(slot.U4)
				};
			}
			else
			{
				ctx.Stack[sp] = new VMSlot
				{
					U4 = slot.U4
				};
			}
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
