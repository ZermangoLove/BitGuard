using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000044 RID: 68
	internal class LindWord : IOpCode
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000073B8 File Offset: 0x000055B8
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_WORD;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000073D0 File Offset: 0x000055D0
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			VMSlot valSlot;
			if (flag)
			{
				valSlot = ((IReference)adrSlot.O).GetValue(ctx, PointerType.WORD);
			}
			else
			{
				ushort* ptr = adrSlot.U8;
				valSlot = new VMSlot
				{
					U2 = *ptr
				};
			}
			ctx.Stack[sp] = valSlot;
			state = ExecutionState.Next;
		}
	}
}
