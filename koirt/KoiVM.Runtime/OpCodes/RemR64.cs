using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200003F RID: 63
	internal class RemR64 : IOpCode
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00006F0C File Offset: 0x0000510C
		public byte Code
		{
			get
			{
				return Constants.OP_REM_R64;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006F24 File Offset: 0x00005124
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.R8 = op2Slot.R8 % op1Slot.R8;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1 & ~mask;
			bool flag = slot.R8 == 0.0;
			if (flag)
			{
				fl |= Constants.FL_ZERO;
			}
			else
			{
				bool flag2 = slot.R8 < 0.0;
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
