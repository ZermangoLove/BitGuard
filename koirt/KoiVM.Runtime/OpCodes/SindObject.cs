using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004D RID: 77
	internal class SindObject : IOpCode
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00007A50 File Offset: 0x00005C50
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_OBJECT;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007A68 File Offset: 0x00005C68
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp--];
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = adrSlot.O is IReference;
			if (flag)
			{
				((IReference)adrSlot.O).SetValue(ctx, valSlot, PointerType.OBJECT);
				state = ExecutionState.Next;
				return;
			}
			throw new ExecutionEngineException();
		}
	}
}
