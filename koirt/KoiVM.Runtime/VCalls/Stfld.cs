using System;
using System.Reflection;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000011 RID: 17
	internal class Stfld : IVCall
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000036CC File Offset: 0x000018CC
		public byte Code
		{
			get
			{
				return Constants.VCALL_STFLD;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000036E4 File Offset: 0x000018E4
		public unsafe void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot fieldSlot = ctx.Stack[sp--];
			VMSlot valSlot = ctx.Stack[sp--];
			VMSlot objSlot = ctx.Stack[sp--];
			FieldInfo field = (FieldInfo)ctx.Instance.Data.LookupReference(fieldSlot.U4);
			bool flag = !field.IsStatic && objSlot.O == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			bool flag2 = Type.GetTypeCode(field.FieldType) == TypeCode.String && valSlot.O == null;
			object value;
			if (flag2)
			{
				value = ctx.Instance.Data.LookupString(valSlot.U4);
			}
			else
			{
				value = valSlot.ToObject(field.FieldType);
			}
			bool flag3 = field.DeclaringType.IsValueType && objSlot.O is IReference;
			if (flag3)
			{
				TypedReference typedRef;
				((IReference)objSlot.O).ToTypedReference(ctx, (void*)(&typedRef), field.DeclaringType);
				TypedReferenceHelpers.CastTypedRef((void*)(&typedRef), field.DeclaringType);
				field.SetValueDirect(typedRef, value);
			}
			else
			{
				field.SetValue(objSlot.ToObject(field.DeclaringType), value);
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
