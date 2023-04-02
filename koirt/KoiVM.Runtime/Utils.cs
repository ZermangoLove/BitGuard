using System;
using KoiVM.Runtime.Dynamic;

namespace KoiVM.Runtime
{
	// Token: 0x02000003 RID: 3
	internal static class Utils
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
		public unsafe static uint ReadCompressedUInt(ref byte* ptr)
		{
			uint num = 0U;
			int shift = 0;
			byte* ptr2;
			do
			{
				num |= (uint)((uint)(*ptr & 127) << shift);
				shift += 7;
				ptr2 = ptr;
				ptr = ptr2 + 1;
			}
			while ((*ptr2 & 128) > 0);
			return num;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020B0 File Offset: 0x000002B0
		public static uint FromCodedToken(uint codedToken)
		{
			uint rid = codedToken >> 3;
			uint num;
			switch (codedToken & 7U)
			{
			case 1U:
				num = rid | 33554432U;
				break;
			case 2U:
				num = rid | 16777216U;
				break;
			case 3U:
				num = rid | 452984832U;
				break;
			case 4U:
				num = rid | 167772160U;
				break;
			case 5U:
				num = rid | 100663296U;
				break;
			case 6U:
				num = rid | 67108864U;
				break;
			case 7U:
				num = rid | 721420288U;
				break;
			default:
				num = rid;
				break;
			}
			return num;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000213C File Offset: 0x0000033C
		public static void UpdateFL(uint op1, uint op2, uint flResult, uint result, ref byte fl, byte mask)
		{
			byte flag = 0;
			bool flag2 = result == 0U;
			if (flag2)
			{
				flag |= Constants.FL_ZERO;
			}
			bool flag3 = ((ulong)result & (ulong)int.MinValue) > 0UL;
			if (flag3)
			{
				flag |= Constants.FL_SIGN;
			}
			bool flag4 = ((ulong)((op1 ^ flResult) & (op2 ^ flResult)) & (ulong)int.MinValue) > 0UL;
			if (flag4)
			{
				flag |= Constants.FL_OVERFLOW;
			}
			bool flag5 = ((ulong)(op1 ^ ((op1 ^ op2) & (op2 ^ flResult))) & (ulong)int.MinValue) > 0UL;
			if (flag5)
			{
				flag |= Constants.FL_CARRY;
			}
			fl = (fl & ~mask) | (flag & mask);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021CC File Offset: 0x000003CC
		public static void UpdateFL(ulong op1, ulong op2, ulong flResult, ulong result, ref byte fl, byte mask)
		{
			byte flag = 0;
			bool flag2 = result == 0UL;
			if (flag2)
			{
				flag |= Constants.FL_ZERO;
			}
			bool flag3 = (result & (ulong)int.MinValue) > 0UL;
			if (flag3)
			{
				flag |= Constants.FL_SIGN;
			}
			bool flag4 = ((op1 ^ flResult) & (op2 ^ flResult) & (ulong)int.MinValue) > 0UL;
			if (flag4)
			{
				flag |= Constants.FL_OVERFLOW;
			}
			bool flag5 = ((op1 ^ ((op1 ^ op2) & (op2 ^ flResult))) & (ulong)int.MinValue) > 0UL;
			if (flag5)
			{
				flag |= Constants.FL_CARRY;
			}
			fl = (fl & ~mask) | (flag & mask);
		}
	}
}
