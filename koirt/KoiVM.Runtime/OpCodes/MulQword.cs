using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000036 RID: 54
	internal class MulQword : IOpCode
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000063BC File Offset: 0x000045BC
		public byte Code
		{
			get
			{
				return Constants.OP_MUL_QWORD;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000063D4 File Offset: 0x000045D4
		private static ulong Carry(ulong a, ulong b)
		{
			ulong lo = a & (ulong)(-1);
			ulong hi = a >> 32;
			ulong lo2 = b & (ulong)(-1);
			ulong hi2 = b >> 32;
			ulong x = lo * lo2;
			ulong s0 = x & (ulong)(-1);
			x = hi * lo2 + (x >> 32);
			ulong s = x & (ulong)(-1);
			ulong s2 = x >> 32;
			x = s + lo * hi2;
			s = x & (ulong)(-1);
			return s2 + hi * hi2 + (x >> 32);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006440 File Offset: 0x00004640
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			VMSlot slot = default(VMSlot);
			ulong result = op1Slot.U8 * op2Slot.U8;
			slot.U8 = result;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			byte mask2 = Constants.FL_CARRY | Constants.FL_OVERFLOW;
			byte ovF = 0;
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				bool flag2 = MulQword.Carry(op1Slot.U8, op2Slot.U8) > 0UL;
				if (flag2)
				{
					ovF = mask2;
				}
			}
			else
			{
				bool flag3 = result >> 63 != (op1Slot.U8 ^ op2Slot.U8) >> 63;
				if (flag3)
				{
					ovF = mask2;
				}
			}
			fl = (fl & ~mask2) | ovF;
			Utils.UpdateFL((ulong)op1Slot.U4, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
