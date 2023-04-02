using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000047 RID: 71
	internal class LindObject : IOpCode
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000075B0 File Offset: 0x000057B0
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_OBJECT;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000075C8 File Offset: 0x000057C8
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			if (flag)
			{
				VMSlot valSlot = ((IReference)adrSlot.O).GetValue(ctx, PointerType.OBJECT);
				ctx.Stack[sp] = valSlot;
				state = ExecutionState.Next;
				return;
			}
			throw new ExecutionEngineException();
		}
	}
}
