using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004E RID: 78
	internal class SindPtr : IOpCode
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00007B08 File Offset: 0x00005D08
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_PTR;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007B20 File Offset: 0x00005D20
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp--];
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			bool flag = adrSlot.O is IReference;
			if (flag)
			{
				((IReference)adrSlot.O).SetValue(ctx, valSlot, Platform.x64 ? PointerType.QWORD : PointerType.DWORD);
			}
			else
			{
				bool x = Platform.x64;
				if (x)
				{
					ulong* ptr = adrSlot.U8;
					*ptr = valSlot.U8;
				}
				else
				{
					uint* ptr2 = adrSlot.U8;
					*ptr2 = valSlot.U4;
				}
			}
			state = ExecutionState.Next;
		}
	}
}
