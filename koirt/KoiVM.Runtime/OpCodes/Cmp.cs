using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200001F RID: 31
	internal class Cmp : IOpCode
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004E84 File Offset: 0x00003084
		public byte Code
		{
			get
			{
				return Constants.OP_CMP;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004E9C File Offset: 0x0000309C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 2U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = op1Slot.O == op2Slot.O;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = -1;
			}
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1 & ~mask;
			bool flag2 = result == 0;
			if (flag2)
			{
				fl |= Constants.FL_ZERO;
			}
			else
			{
				bool flag3 = result < 0;
				if (flag3)
				{
					fl |= Constants.FL_SIGN;
				}
			}
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
