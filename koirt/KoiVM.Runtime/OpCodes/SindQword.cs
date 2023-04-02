using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004C RID: 76
	internal class SindQword : IOpCode
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00007984 File Offset: 0x00005B84
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_QWORD;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000799C File Offset: 0x00005B9C
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp--];
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = adrSlot.O is IReference;
			if (flag)
			{
				((IReference)adrSlot.O).SetValue(ctx, valSlot, PointerType.QWORD);
			}
			else
			{
				ulong value = valSlot.U8;
				ulong* ptr = adrSlot.U8;
				*ptr = value;
			}
			state = ExecutionState.Next;
		}
	}
}
