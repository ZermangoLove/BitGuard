using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000018 RID: 24
	internal class AddQword : IOpCode
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004644 File Offset: 0x00002844
		public byte Code
		{
			get
			{
				return Constants.OP_ADD_QWORD;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000465C File Offset: 0x0000285C
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
				slot.O = ((IReference)op1Slot.O).Add(op2Slot.U8);
			}
			else
			{
				bool flag2 = op2Slot.O is IReference;
				if (flag2)
				{
					slot.O = ((IReference)op2Slot.O).Add(op1Slot.U8);
				}
				else
				{
					slot.U8 = op2Slot.U8 + op1Slot.U8;
				}
			}
			ctx.Stack[sp] = slot;
			byte mask = Constants.FL_ZERO | Constants.FL_SIGN | Constants.FL_OVERFLOW | Constants.FL_CARRY;
			byte fl = ctx.Registers[(int)Constants.REG_FL].U1;
			Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
			ctx.Registers[(int)Constants.REG_FL].U1 = fl;
			state = ExecutionState.Next;
		}
	}
}
