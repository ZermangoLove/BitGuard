using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000037 RID: 55
	internal class MulR32 : IOpCode
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000065C0 File Offset: 0x000047C0
		public byte Code
		{
			get
			{
				return Constants.OP_MUL_R32;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000065D8 File Offset: 0x000047D8
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.R4 = op2Slot.R4 * op1Slot.R4;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
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
