using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000007 RID: 7
	internal class Cast : IVCall
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002C88 File Offset: 0x00000E88
		public byte Code
		{
			get
			{
				return Constants.VCALL_CAST;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot typeSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp];
			bool castclass = (typeSlot.U4 & 2147483648U) > 0U;
			Type castType = (Type)ctx.Instance.Data.LookupReference(typeSlot.U4 & 2147483647U);
			bool flag = Type.GetTypeCode(castType) == TypeCode.String && valSlot.O == null;
			if (flag)
			{
				valSlot.O = ctx.Instance.Data.LookupString(valSlot.U4);
			}
			else
			{
				bool flag2 = valSlot.O == null;
				if (flag2)
				{
					valSlot.O = null;
				}
				else
				{
					bool flag3 = !castType.IsInstanceOfType(valSlot.O);
					if (flag3)
					{
						valSlot.O = null;
						bool flag4 = castclass;
						if (flag4)
						{
							throw new InvalidCastException();
						}
					}
				}
			}
			ctx.Stack[sp] = valSlot;
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
