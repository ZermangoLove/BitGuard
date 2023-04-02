using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.OpCodes
{
	// Token: 0x0200003A RID: 58
	internal class Try : IOpCode
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00006960 File Offset: 0x00004B60
		public byte Code
		{
			get
			{
				return Constants.OP_TRY;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006978 File Offset: 0x00004B78
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			byte type = ctx.Stack[sp--].U1;
			EHFrame frame = default(EHFrame);
			frame.EHType = type;
			bool flag = type == Constants.EH_CATCH;
			if (flag)
			{
				frame.CatchType = (Type)ctx.Instance.Data.LookupReference(ctx.Stack[sp--].U4);
			}
			else
			{
				bool flag2 = type == Constants.EH_FILTER;
				if (flag2)
				{
					frame.FilterAddr = ctx.Stack[sp--].U8;
				}
			}
			frame.HandlerAddr = ctx.Stack[sp--].U8;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			frame.BP = ctx.Registers[(int)Constants.REG_BP];
			frame.SP = ctx.Registers[(int)Constants.REG_SP];
			ctx.EHStack.Add(frame);
			state = ExecutionState.Next;
		}
	}
}
