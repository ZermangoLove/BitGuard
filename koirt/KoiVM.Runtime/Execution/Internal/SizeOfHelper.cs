using System;
using System.Collections;
using System.Reflection.Emit;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006E RID: 110
	internal class SizeOfHelper
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0000A55C File Offset: 0x0000875C
		public static int SizeOf(Type type)
		{
			object size = SizeOfHelper.sizes[type];
			bool flag = size == null;
			if (flag)
			{
				Hashtable hashtable = SizeOfHelper.sizes;
				lock (hashtable)
				{
					size = SizeOfHelper.sizes[type];
					bool flag2 = size == null;
					if (flag2)
					{
						size = SizeOfHelper.GetSize(type);
						SizeOfHelper.sizes[type] = size;
					}
				}
			}
			return (int)size;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A5E4 File Offset: 0x000087E4
		private static int GetSize(Type type)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(int), Type.EmptyTypes, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Sizeof, type);
			gen.Emit(OpCodes.Ret);
			return (int)dm.Invoke(null, null);
		}

		// Token: 0x0400003E RID: 62
		private static Hashtable sizes = new Hashtable();
	}
}
