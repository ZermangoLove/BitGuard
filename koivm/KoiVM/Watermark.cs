using System;
using System.Reflection;

namespace KoiVM
{
	// Token: 0x0200000D RID: 13
	[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
	internal static class Watermark
	{
		// Token: 0x0600005D RID: 93 RVA: 0x0000581C File Offset: 0x00003A1C
		internal static byte[] GenerateWatermark(uint rand)
		{
			uint id = 65536U;
			uint a = id * 2492804249U;
			uint b = id * 3131742247U;
			uint c = id * 1865781987U;
			uint d = a + b + c;
			return new byte[]
			{
				(byte)(a >> 24),
				(byte)(a >> 16),
				(byte)(a >> 8),
				(byte)a,
				(byte)(b >> 24),
				(byte)(b >> 16),
				(byte)(b >> 8),
				(byte)b,
				(byte)(c >> 24),
				(byte)(c >> 16),
				(byte)(c >> 8),
				(byte)c,
				(byte)(d >> 24),
				(byte)(d >> 16),
				(byte)(d >> 8),
				(byte)d
			};
		}
	}
}
