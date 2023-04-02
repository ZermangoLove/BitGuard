using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200002F RID: 47
	internal class DivDword : IOpCode
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005AFC File Offset: 0x00003CFC
		public byte Code
		{
			get
			{
				return Constants.OP_DIV_DWORD;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005B14 File Offset: 0x00003D14
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
			bool flag = (fl & Constants.FL_UNSIGNED) > 0;
			if (flag)
			{
				slot.U4 = op1Slot.U4 / op2Slot.U4;
			}
			else
			{
				slot.U4 = op1Slot.U4 / op2Slot.U4;
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
