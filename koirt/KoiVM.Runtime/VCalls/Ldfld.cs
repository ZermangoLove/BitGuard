using System;
using System.Reflection;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000012 RID: 18
	internal class Ldfld : IVCall
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003878 File Offset: 0x00001A78
		public byte Code
		{
			get
			{
				return Constants.VCALL_LDFLD;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003890 File Offset: 0x00001A90
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot fieldSlot = ctx.Stack[sp--];
			VMSlot objSlot = ctx.Stack[sp];
			bool addr = (fieldSlot.U4 & 2147483648U) > 0U;
			FieldInfo field = (FieldInfo)ctx.Instance.Data.LookupReference(fieldSlot.U4 & 2147483647U);
			bool flag = !field.IsStatic && objSlot.O == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			bool flag2 = addr;
			if (flag2)
			{
				ctx.Stack[sp] = new VMSlot
				{
					O = new FieldRef(objSlot.O, field)
				};
			}
			else
			{
				bool flag3 = field.DeclaringType.IsValueType && objSlot.O is IReference;
				object instance;
				if (flag3)
				{
					instance = ((IReference)objSlot.O).GetValue(ctx, PointerType.OBJECT).ToObject(field.DeclaringType);
				}
				else
				{
					instance = objSlot.ToObject(field.DeclaringType);
				}
				ctx.Stack[sp] = VMSlot.FromObject(field.GetValue(instance), field.FieldType);
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
