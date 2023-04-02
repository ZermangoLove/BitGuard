using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000040 RID: 64
	internal class NorDword : IOpCode
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00007054 File Offset: 0x00005254
		public byte Code
		{
			get
			{
				return Constants.OP_NOR_DWORD;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000706C File Offset: 0x0000526C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			slot.U4 = ~(op1Slot.U4 | op2Slot.U4);
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
