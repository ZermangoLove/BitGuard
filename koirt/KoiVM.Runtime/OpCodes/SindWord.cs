using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004A RID: 74
	internal class SindWord : IOpCode
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000077EC File Offset: 0x000059EC
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_WORD;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007804 File Offset: 0x00005A04
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
				((IReference)adrSlot.O).SetValue(ctx, valSlot, PointerType.WORD);
			}
			else
			{
				ushort value = valSlot.U2;
				ushort* ptr = adrSlot.U8;
				*ptr = value;
			}
			state = ExecutionState.Next;
		}
	}
}
