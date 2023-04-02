using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000048 RID: 72
	internal class LindPtr : IOpCode
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000763C File Offset: 0x0000583C
		public byte Code
		{
			get
			{
				return Constants.OP_LIND_PTR;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007654 File Offset: 0x00005854
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot adrSlot = ctx.Stack[sp];
			bool flag = adrSlot.O is IReference;
			VMSlot valSlot;
			if (flag)
			{
				valSlot = ((IReference)adrSlot.O).GetValue(ctx, Platform.x64 ? PointerType.QWORD : PointerType.DWORD);
			}
			else
			{
				bool x = Platform.x64;
				if (x)
				{
					ulong* ptr = adrSlot.U8;
					valSlot = new VMSlot
					{
						U8 = *ptr
					};
				}
				else
				{
					uint* ptr2 = adrSlot.U8;
					valSlot = new VMSlot
					{
						U4 = *ptr2
					};
				}
			}
			ctx.Stack[sp] = valSlot;
			state = ExecutionState.Next;
		}
	}
}
