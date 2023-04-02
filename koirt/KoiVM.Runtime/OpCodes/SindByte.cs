using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000049 RID: 73
	internal class SindByte : IOpCode
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00007720 File Offset: 0x00005920
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_BYTE;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007738 File Offset: 0x00005938
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
				((IReference)adrSlot.O).SetValue(ctx, valSlot, PointerType.BYTE);
			}
			else
			{
				byte value = valSlot.U1;
				byte* ptr = adrSlot.U8;
				*ptr = value;
			}
			state = ExecutionState.Next;
		}
	}
}
