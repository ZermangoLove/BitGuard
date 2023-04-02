using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200005A RID: 90
	internal class PushIDword : IOpCode
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000082FC File Offset: 0x000064FC
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHI_DWORD;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008314 File Offset: 0x00006514
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			ulong imm = (ulong)ctx.ReadByte();
			imm |= (ulong)ctx.ReadByte() << 8;
			imm |= (ulong)ctx.ReadByte() << 16;
			imm |= (ulong)ctx.ReadByte() << 24;
			ulong sx = (((imm & (ulong)int.MinValue) != 0UL) ? 18446744069414584320UL : 0UL);
			ctx.Stack[sp] = new VMSlot
			{
				U8 = (sx | imm)
			};
			state = ExecutionState.Next;
		}
	}
}
