using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000035 RID: 53
	internal class MulDword : IOpCode
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00006224 File Offset: 0x00004424
		public byte Code
		{
			get
			{
				return Constants.OP_MUL_DWORD;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000623C File Offset: 0x0000443C
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
			ulong result = (ulong)(op1Slot.U4 * op2Slot.U4);
			slot.U4 = (uint)result;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			byte mask2 = Constants.FL_CARRY | Constants.FL_OVERFLOW;
			byte ovF = 0;
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				bool flag2 = (result & (ulong)(-1)) > 0UL;
				if (flag2)
				{
					ovF = mask2;
				}
			}
			else
			{
				result = (ulong)((long)(op1Slot.U4 * op2Slot.U4));
				bool flag3 = result >> 63 != (ulong)(slot.U4 >> 31);
				if (flag3)
				{
					ovF = mask2;
				}
			}
			fl = (fl & ~mask2) | ovF;
			Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
