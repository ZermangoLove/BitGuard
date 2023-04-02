using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200003D RID: 61
	internal class RemQword : IOpCode
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00006C88 File Offset: 0x00004E88
		public byte Code
		{
			get
			{
				return Constants.OP_REM_QWORD;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006CA0 File Offset: 0x00004EA0
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
				slot.U8 = op1Slot.U8 % op2Slot.U8;
			}
			else
			{
				slot.U8 = op1Slot.U8 % op2Slot.U8;
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
