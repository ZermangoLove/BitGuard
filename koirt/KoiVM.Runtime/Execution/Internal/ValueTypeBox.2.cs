using System;
using System.Diagnostics;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006D RID: 109
	internal static class ValueTypeBox
	{
		// Token: 0x06000177 RID: 375 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static IValueTypeBox Box(object vt, Type vtType)
		{
			Debug.Assert(vtType.IsValueType);
			Type boxType = typeof(ValueTypeBox<>).MakeGenericType(new Type[] { vtType });
			return (IValueTypeBox)Activator.CreateInstance(boxType, new object[] { vt });
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A52C File Offset: 0x0000872C
		public static object Unbox(object box)
		{
			bool flag = box is IValueTypeBox;
			object obj;
			if (flag)
			{
				obj = ((IValueTypeBox)box).GetValue();
			}
			else
			{
				obj = box;
			}
			return obj;
		}
	}
}
