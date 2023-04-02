using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200002E RID: 46
	internal class ShrQword : IOpCode
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000059B4 File Offset: 0x00003BB4
		public byte Code
		{
			get
			{
				return Constants.OP_SHR_QWORD;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000059CC File Offset: 0x00003BCC
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
				slot.U8 = op1Slot.U8 >> (int)op2Slot.U4;
			}
			else
			{
				slot.U8 = op1Slot.U8 >> (int)op2Slot.U4;
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
