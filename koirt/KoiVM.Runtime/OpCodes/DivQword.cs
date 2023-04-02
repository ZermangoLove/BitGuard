using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000030 RID: 48
	internal class DivQword : IOpCode
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005C40 File Offset: 0x00003E40
		public byte Code
		{
			get
			{
				return Constants.OP_DIV_QWORD;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005C58 File Offset: 0x00003E58
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
				slot.U8 = op1Slot.U8 / op2Slot.U8;
			}
			else
			{
				slot.U8 = (ulong)((int)op1Slot.U8 / (int)op2Slot.U8);
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_UNSIGNED;
			Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
