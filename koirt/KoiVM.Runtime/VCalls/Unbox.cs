using System;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000013 RID: 19
	internal class Unbox : IVCall
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003A0C File Offset: 0x00001C0C
		public byte Code
		{
			get
			{
				return Constants.VCALL_UNBOX;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003A24 File Offset: 0x00001C24
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot typeSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp];
			bool unboxPtr = (typeSlot.U4 & 2147483648U) > 0U;
			Type valType = (Type)ctx.Instance.Data.LookupReference(typeSlot.U4 & 2147483647U);
			bool flag = unboxPtr;
			if (flag)
			{
				TypedReference typedRef;
				TypedReferenceHelpers.UnboxTypedRef(valSlot.O, (void*)(&typedRef));
				TypedRef reference = new TypedRef(typedRef);
				valSlot = VMSlot.FromObject(valSlot.O, valType);
				ctx.Stack[sp] = valSlot;
			}
			else
			{
				bool flag2 = valType == typeof(object) && valSlot.O != null;
				if (flag2)
				{
					valType = valSlot.O.GetType();
				}
				valSlot = VMSlot.FromObject(valSlot.O, valType);
				ctx.Stack[sp] = valSlot;
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
