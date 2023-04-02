using System;
using System.Runtime.Serialization;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x0200000D RID: 13
	internal class Initobj : IVCall
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003160 File Offset: 0x00001360
		public byte Code
		{
			get
			{
				return Constants.VCALL_INITOBJ;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003178 File Offset: 0x00001378
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot typeSlot = ctx.Stack[sp--];
			VMSlot addrSlot = ctx.Stack[sp--];
			Type type = (Type)ctx.Instance.Data.LookupReference(typeSlot.U4);
			bool flag = addrSlot.O is IReference;
			if (flag)
			{
				IReference reference = (IReference)addrSlot.O;
				VMSlot slot = default(VMSlot);
				bool isValueType = type.IsValueType;
				if (isValueType)
				{
					object def = null;
					bool flag2 = Nullable.GetUnderlyingType(type) == null;
					if (flag2)
					{
						def = FormatterServices.GetUninitializedObject(type);
					}
					slot.O = ValueTypeBox.Box(def, type);
				}
				else
				{
					slot.O = null;
				}
				reference.SetValue(ctx, slot, PointerType.OBJECT);
				ctx.Stack.SetTopPosition(sp);
				ctx.Registers[(int)Constants.REG_SP].U4 = sp;
				state = ExecutionState.Next;
				return;
			}
			throw new NotSupportedException();
		}
	}
}
