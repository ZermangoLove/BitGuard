using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200001B RID: 27
	internal class CmpDword : IOpCode
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00004A5C File Offset: 0x00002C5C
		public byte Code
		{
			get
			{
				return Constants.OP_CMP_DWORD;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004A74 File Offset: 0x00002C74
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 2U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			uint result = op1Slot.U4 - op2Slot.U4;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			Utils.UpdateFL(result, op2Slot.U4, op1Slot.U4, result, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
