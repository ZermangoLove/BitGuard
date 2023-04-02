using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000034 RID: 52
	internal class ShlQword : IOpCode
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000610C File Offset: 0x0000430C
		public byte Code
		{
			get
			{
				return Constants.OP_SHL_QWORD;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006124 File Offset: 0x00004324
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.U8 = op1Slot.U8 << (int)op2Slot.U4;
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
