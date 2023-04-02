using System;
using System.Reflection;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000E RID: 14
	internal class Token : IVCall
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003288 File Offset: 0x00001488
		public byte Code
		{
			get
			{
				return Constants.VCALL_TOKEN;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000032A0 File Offset: 0x000014A0
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot typeSlot = ctx.Stack[sp];
			MemberInfo reference = ctx.Instance.Data.LookupReference(typeSlot.U4);
			bool flag = reference is Type;
			if (flag)
			{
				typeSlot.O = ValueTypeBox.Box(((Type)reference).TypeHandle, typeof(RuntimeTypeHandle));
			}
			else
			{
				bool flag2 = reference is MethodBase;
				if (flag2)
				{
					typeSlot.O = ValueTypeBox.Box(((MethodBase)reference).MethodHandle, typeof(RuntimeMethodHandle));
				}
				else
				{
					bool flag3 = reference is FieldInfo;
					if (flag3)
					{
						typeSlot.O = ValueTypeBox.Box(((FieldInfo)reference).FieldHandle, typeof(RuntimeFieldHandle));
					}
				}
			}
			ctx.Stack[sp] = typeSlot;
			state = ExecutionState.Next;
		}
	}
}
