using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200003E RID: 62
	internal class RemR32 : IOpCode
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006DCC File Offset: 0x00004FCC
		public byte Code
		{
			get
			{
				return Constants.OP_REM_R32;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006DE4 File Offset: 0x00004FE4
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.R4 = op2Slot.R4 % op1Slot.R4;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1 & ~mask;
			bool flag = slot.R4 == 0f;
			if (flag)
			{
				fl |= Constants.FL_ZERO;
			}
			else
			{
				bool flag2 = slot.R4 < 0f;
				if (flag2)
				{
					fl |= Constants.FL_SIGN;
				}
			}
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
