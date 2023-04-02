using System;
using System.Collections.Generic;
using KoiVM.Runtime.VCalls;

namespace KoiVM.Runtime.Data
{
	// Token: 0x02000075 RID: 117
	internal static class VCallMap
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000BF3C File Offset: 0x0000A13C
		static VCallMap()
		{
			foreach (Type type in typeof(VCallMap).Assembly.GetTypes())
			{
				bool flag = typeof(IVCall).IsAssignableFrom(type) && !type.IsAbstract;
				if (flag)
				{
					IVCall vCall = (IVCall)Activator.CreateInstance(type);
					VCallMap.vCalls[vCall.Code] = vCall;
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		public static IVCall Lookup(byte code)
		{
			return VCallMap.vCalls[code];
		}

		// Token: 0x040000C8 RID: 200
		private static readonly Dictionary<byte, IVCall> vCalls = new Dictionary<byte, IVCall>();
	}
}
