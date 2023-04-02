using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000046 RID: 70
	internal class LindQword : IOpCode
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007508 File Offset: 0x00005708
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_QWORD;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007520 File Offset: 0x00005720
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			VMSlot valSlot;
			if (flag)
			{
				valSlot = ((IReference)adrSlot.O).GetValue(ctx, PointerType.QWORD);
			}
			else
			{
				ulong* ptr = adrSlot.U8;
				valSlot = new VMSlot
				{
					U8 = *ptr
				};
			}
			ctx.Stack[sp] = valSlot;
			state = ExecutionState.Next;
		}
	}
}
