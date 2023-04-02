using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000043 RID: 67
	internal class LindByte : IOpCode
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00007310 File Offset: 0x00005510
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_BYTE;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007328 File Offset: 0x00005528
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			VMSlot valSlot;
			if (flag)
			{
				valSlot = ((IReference)adrSlot.O).GetValue(ctx, PointerType.BYTE);
			}
			else
			{
				byte* ptr = adrSlot.U8;
				valSlot = new VMSlot
				{
					U1 = *ptr
				};
			}
			ctx.Stack[sp] = valSlot;
			state = ExecutionState.Next;
		}
	}
}
