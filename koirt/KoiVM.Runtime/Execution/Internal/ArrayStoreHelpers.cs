using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006F RID: 111
	internal class ArrayStoreHelpers
	{
		// Token: 0x0600017D RID: 381 RVA: 0x0000A650 File Offset: 0x00008850
		public static void SetValue(Array array, int index, object value, Type valueType, Type elemType)
		{
			Debug.Assert(value == null || value.GetType() == valueType);
			KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(valueType, elemType);
			object helper = ArrayStoreHelpers.storeHelpers[key];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = ArrayStoreHelpers.storeHelpers;
				lock (hashtable)
				{
					helper = ArrayStoreHelpers.storeHelpers[key];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = ArrayStoreHelpers.BuildStoreHelper(valueType, elemType);
						ArrayStoreHelpers.storeHelpers[key] = helper;
					}
				}
			}
			((ArrayStoreHelpers._SetValue)helper)(array, index, value);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A708 File Offset: 0x00008908
		private static ArrayStoreHelpers._SetValue BuildStoreHelper(Type valueType, Type elemType)
		{
			Type[] paramTypes = new Type[]
			{
				typeof(Array),
				typeof(int),
				typeof(object)
			};
			DynamicMethod dm = new DynamicMethod("", typeof(void), paramTypes, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Ldarg_2);
			bool isValueType = elemType.IsValueType;
			if (isValueType)
			{
				gen.Emit(OpCodes.Unbox_Any, valueType);
			}
			gen.Emit(OpCodes.Stelem, elemType);
			gen.Emit(OpCodes.Ret);
			return (ArrayStoreHelpers._SetValue)dm.CreateDelegate(typeof(ArrayStoreHelpers._SetValue));
		}

		// Token: 0x0400003F RID: 63
		private static Hashtable storeHelpers = new Hashtable();

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x060001B6 RID: 438
		private delegate void _SetValue(Array array, int index, object value);
	}
}
