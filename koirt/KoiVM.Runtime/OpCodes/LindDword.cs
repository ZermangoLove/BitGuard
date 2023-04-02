using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000045 RID: 69
	internal class LindDword : IOpCode
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00007460 File Offset: 0x00005660
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_DWORD;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007478 File Offset: 0x00005678
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			VMSlot valSlot;
			if (flag)
			{
				valSlot = ((IReference)adrSlot.O).GetValue(ctx, PointerType.DWORD);
			}
			else
			{
				uint* ptr = adrSlot.U8;
				valSlot = new VMSlot
				{
					U4 = *ptr
				};
			}
			ctx.Stack[sp] = valSlot;
			state = ExecutionState.Next;
		}
	}
}
