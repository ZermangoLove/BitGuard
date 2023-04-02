using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000B RID: 11
	internal class Sizeof : IVCall
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003020 File Offset: 0x00001220
		public byte Code
		{
			get
			{
				return Constants.VCALL_SIZEOF;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003038 File Offset: 0x00001238
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			uint bp = ctx.Registers[(int)Constants.REG_BP].U4;
			Type type = (Type)ctx.Instance.Data.LookupReference(ctx.Stack[sp].U4);
			ctx.Stack[sp] = new VMSlot
			{
				U4 = (uint)SizeOfHelper.SizeOf(type)
			};
			state = ExecutionState.Next;
		}
	}
}
