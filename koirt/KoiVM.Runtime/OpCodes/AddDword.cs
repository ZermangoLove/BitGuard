using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000017 RID: 23
	internal class AddDword : IOpCode
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000044B4 File Offset: 0x000026B4
		public byte Code
		{
			get
			{
				return Constants.OP_ADD_DWORD;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000044CC File Offset: 0x000026CC
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot op1Slot = ctx.Stack[sp - 1U];
			VMSlot op2Slot = ctx.Stack[sp];
			sp -= 1U;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			VMSlot slot = default(VMSlot);
			bool flag = op1Slot.O is IReference;
			if (flag)
			{
				slot.O = ((IReference)op1Slot.O).Add(op2Slot.U4);
			}
			else
			{
				bool flag2 = op2Slot.O is IReference;
				if (flag2)
				{
					slot.O = ((IReference)op2Slot.O).Add(op1Slot.U4);
				}
				else
				{
					slot.U4 = op2Slot.U4 + op1Slot.U4;
				}
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
