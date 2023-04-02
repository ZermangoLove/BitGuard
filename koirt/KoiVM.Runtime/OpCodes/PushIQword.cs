using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200005B RID: 91
	internal class PushIQword : IOpCode
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000083CC File Offset: 0x000065CC
		public byte Code
		{
			get
			{
				return Constants.OP_PUSHI_QWORD;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000083E4 File Offset: 0x000065E4
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ctx.Stack.SetTopPosition(sp += 1U);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			ulong imm = (ulong)ctx.ReadByte();
			imm |= (ulong)ctx.ReadByte() << 8;
			imm |= (ulong)ctx.ReadByte() << 16;
			imm |= (ulong)ctx.ReadByte() << 24;
			imm |= (ulong)ctx.ReadByte() << 32;
			imm |= (ulong)ctx.ReadByte() << 40;
			imm |= (ulong)ctx.ReadByte() << 48;
			imm |= (ulong)ctx.ReadByte() << 56;
			ctx.Stack[sp] = new VMSlot
			{
				U8 = imm
			};
			state = ExecutionState.Next;
		}
	}
}
