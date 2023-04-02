using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x02000039 RID: 57
	internal class Leave : IOpCode
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00006838 File Offset: 0x00004A38
		public byte Code
		{
			get
			{
				return Constants.OP_LEAVE;
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006850 File Offset: 0x00004A50
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			ulong handler = ctx.Stack[sp--].U8;
			int frameIndex = ctx.EHStack.Count - 1;
			EHFrame frame = ctx.EHStack[frameIndex];
			bool flag = frame.HandlerAddr != handler;
			if (flag)
			{
				throw new InvalidProgramException();
			}
			ctx.EHStack.RemoveAt(frameIndex);
			bool flag2 = frame.EHType == Constants.EH_FINALLY;
			if (flag2)
			{
				ctx.Stack[sp += 1U] = ctx.Registers[(int)Constants.REG_IP];
				ctx.Registers[(int)Constants.REG_K1].U1 = 0;
				ctx.Registers[(int)Constants.REG_IP].U8 = frame.HandlerAddr;
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
