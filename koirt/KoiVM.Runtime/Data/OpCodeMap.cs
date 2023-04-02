using System;
using System.Collections.Generic;
using KoiVM.Runtime.OpCodes;

namespace KoiVM.Runtime.Data
{
	// Token: 0x02000078 RID: 120
	internal static class OpCodeMap
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x0000C298 File Offset: 0x0000A498
		static OpCodeMap()
		{
			foreach (Type type in typeof(OpCodeMap).Assembly.GetTypes())
			{
				bool flag = typeof(IOpCode).IsAssignableFrom(type) && !type.IsAbstract;
				if (flag)
				{
					IOpCode opCode = (IOpCode)Activator.CreateInstance(type);
					OpCodeMap.opCodes[opCode.Code] = opCode;
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000C320 File Offset: 0x0000A520
		public static IOpCode Lookup(byte code)
		{
			return OpCodeMap.opCodes[code];
		}

		// Token: 0x040000D2 RID: 210
		private static readonly Dictionary<byte, IOpCode> opCodes = new Dictionary<byte, IOpCode>();
	}
}
