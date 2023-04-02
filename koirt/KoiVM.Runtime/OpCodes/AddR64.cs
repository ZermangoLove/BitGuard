﻿using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200001A RID: 26
	internal class AddR64 : IOpCode
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004914 File Offset: 0x00002B14
		public byte Code
		{
			get
			{
				return Constants.OP_ADD_R64;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000492C File Offset: 0x00002B2C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.R8 = op2Slot.R8 + op1Slot.R8;
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
