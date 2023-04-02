using System;
using System.Diagnostics;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000006 RID: 6
	internal class Box : IVCall
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002B80 File Offset: 0x00000D80
		public byte Code
		{
			get
			{
				return Constants.VCALL_BOX;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B98 File Offset: 0x00000D98
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot typeSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp];
			Type valType = (Type)ctx.Instance.Data.LookupReference(typeSlot.U4);
			bool flag = Type.GetTypeCode(valType) == TypeCode.String && valSlot.O == null;
			if (flag)
			{
				valSlot.O = ctx.Instance.Data.LookupString(valSlot.U4);
			}
			else
			{
				Debug.Assert(valType.IsValueType);
				valSlot.O = valSlot.ToObject(valType);
			}
			ctx.Stack[sp] = valSlot;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
