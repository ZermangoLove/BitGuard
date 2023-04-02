using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200004B RID: 75
	internal class SindDword : IOpCode
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000078B8 File Offset: 0x00005AB8
		public byte Code
		{
			get
			{
				return Constants.OP_SIND_DWORD;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000078D0 File Offset: 0x00005AD0
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
				((IReference)adrSlot.O).SetValue(ctx, valSlot, PointerType.DWORD);
			}
			else
			{
				uint value = valSlot.U4;
				uint* ptr = adrSlot.U8;
				*ptr = value;
			}
			state = ExecutionState.Next;
		}
	}
}
