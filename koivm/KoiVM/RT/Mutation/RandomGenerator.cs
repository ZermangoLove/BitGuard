using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000F8 RID: 248
	internal class RandomGenerator
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00015F1C File Offset: 0x0001411C
		internal RandomGenerator()
		{
			byte[] seed = new byte[32];
			RandomGenerator._RNG.GetBytes(seed);
			this.state = RandomGenerator._SHA256((byte[])seed.Clone());
			this.seedLen = seed.Length;
			this.stateFilled = 32;
			this.mixIndex = 0;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00015F80 File Offset: 0x00014180
		internal RandomGenerator(int length)
		{
			byte[] seed = new byte[(length == 0) ? 32 : length];
			RandomGenerator._RNG.GetBytes(seed);
			this.state = RandomGenerator._SHA256((byte[])seed.Clone());
			this.seedLen = seed.Length;
			this.stateFilled = 32;
			this.mixIndex = 0;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015FE8 File Offset: 0x000141E8
		internal RandomGenerator(string seed)
		{
			byte[] ret = RandomGenerator._SHA256((byte[])((!string.IsNullOrEmpty(seed)) ? Encoding.UTF8.GetBytes(seed) : Guid.NewGuid().ToByteArray()).Clone());
			for (int i = 0; i < 32; i++)
			{
				byte[] array = ret;
				int num = i;
				byte[] array2 = array;
				int num2 = num;
				array2[num2] *= RandomGenerator.primes[i % RandomGenerator.primes.Length];
				ret = RandomGenerator._SHA256(ret);
			}
			this.state = ret;
			this.seedLen = ret.Length;
			this.stateFilled = 32;
			this.mixIndex = 0;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000311C File Offset: 0x0000131C
		internal RandomGenerator(byte[] seed)
		{
			this.state = (byte[])seed.Clone();
			this.seedLen = seed.Length;
			this.stateFilled = 32;
			this.mixIndex = 0;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00016098 File Offset: 0x00014298
		public static byte[] _SHA256(byte[] buffer)
		{
			SHA256Managed sha = new SHA256Managed();
			return sha.ComputeHash(buffer);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000160B8 File Offset: 0x000142B8
		private void NextState()
		{
			for (int i = 0; i < 32; i++)
			{
				byte[] array = this.state;
				int num = i;
				byte[] array2 = array;
				int num2 = num;
				array2[num2] ^= RandomGenerator.primes[this.mixIndex = (this.mixIndex + 1) % RandomGenerator.primes.Length];
			}
			this.state = this.sha256.ComputeHash(this.state);
			this.stateFilled = 32;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016130 File Offset: 0x00014330
		public void NextBytes(byte[] buffer, int offset, int length)
		{
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			bool flag3 = length < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			bool flag4 = buffer.Length - offset < length;
			if (flag4)
			{
				throw new ArgumentException("Invalid offset or length.");
			}
			while (length > 0)
			{
				bool flag5 = length >= this.stateFilled;
				if (flag5)
				{
					Buffer.BlockCopy(this.state, 32 - this.stateFilled, buffer, offset, this.stateFilled);
					offset += this.stateFilled;
					length -= this.stateFilled;
					this.stateFilled = 0;
				}
				else
				{
					Buffer.BlockCopy(this.state, 32 - this.stateFilled, buffer, offset, length);
					this.stateFilled -= length;
					length = 0;
				}
				bool flag6 = this.stateFilled == 0;
				if (flag6)
				{
					this.NextState();
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00016238 File Offset: 0x00014438
		public byte NextByte()
		{
			byte ret = this.state[32 - this.stateFilled];
			this.stateFilled--;
			bool flag = this.stateFilled == 0;
			if (flag)
			{
				this.NextState();
			}
			return ret;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016280 File Offset: 0x00014480
		public string NextString(int length)
		{
			try
			{
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < length; i++)
				{
					char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(32m + this.NextInt32(122) - 32m)));
					builder.Append(ch);
				}
				return builder.ToString();
			}
			catch
			{
			}
			return string.Empty;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00016310 File Offset: 0x00014510
		public string NextHexString(int length, bool large = false)
		{
			bool flag = length.ToString().Contains("5");
			if (flag)
			{
				throw new Exception("5 is an unacceptable number!");
			}
			try
			{
				string chars = "qwertyuıopğüasdfghjklşizxcvbnmöçQWERTYUIOPĞÜASDFGHJKLŞİZXCVBNMÖÇ0123456789/*-.:,;!'^+%&/()=?_~|\\}][{½$#£>";
				string rnd = new string((from s in Enumerable.Repeat<string>(chars, length / 2)
					select s[this.NextInt32(s.Length)]).ToArray<char>());
				bool flag2 = !large;
				if (flag2)
				{
					return BitConverter.ToString(Encoding.Default.GetBytes(rnd)).Replace("-", string.Empty).ToLower();
				}
				if (large)
				{
					return BitConverter.ToString(Encoding.Default.GetBytes(rnd)).Replace("-", string.Empty);
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000163EC File Offset: 0x000145EC
		public string NextHexString(bool large = false)
		{
			return this.NextHexString(8, large);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00016408 File Offset: 0x00014608
		public string NextString()
		{
			return this.NextString(this.seedLen);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00016428 File Offset: 0x00014628
		public byte[] NextBytes(int length)
		{
			byte[] ret = new byte[length];
			this.NextBytes(ret, 0, length);
			return ret;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001644C File Offset: 0x0001464C
		public byte[] NextBytes()
		{
			byte[] ret = new byte[this.seedLen];
			this.NextBytes(ret, 0, this.seedLen);
			return ret;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001647C File Offset: 0x0001467C
		public int NextInt32()
		{
			return BitConverter.ToInt32(this.NextBytes(4), 0);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001649C File Offset: 0x0001469C
		public int NextInt32(int max)
		{
			return (int)((ulong)this.NextUInt32() % (ulong)((long)max));
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000164BC File Offset: 0x000146BC
		public int NextInt32(int min, int max)
		{
			bool flag = max <= min;
			int num;
			if (flag)
			{
				num = min;
			}
			else
			{
				num = min + (int)((ulong)this.NextUInt32() % (ulong)((long)(max - min)));
			}
			return num;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000164F0 File Offset: 0x000146F0
		public uint NextUInt32()
		{
			return BitConverter.ToUInt32(this.NextBytes(4), 0);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016510 File Offset: 0x00014710
		public uint NextUInt32(uint max)
		{
			return this.NextUInt32() % max;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001652C File Offset: 0x0001472C
		public double NextDouble()
		{
			return this.NextUInt32() / 4294967296.0;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00016550 File Offset: 0x00014750
		public double NextDouble(double min, double max)
		{
			return this.NextDouble() * (max - min) + min;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00016570 File Offset: 0x00014770
		public bool NextBoolean()
		{
			byte s = this.state[32 - this.stateFilled];
			this.stateFilled--;
			bool flag = this.stateFilled == 0;
			if (flag)
			{
				this.NextState();
			}
			return s % 2 == 0;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000165C0 File Offset: 0x000147C0
		public void Shuffle<T>(IList<T> list)
		{
			for (int i = list.Count - 1; i > 1; i--)
			{
				int j = this.NextInt32(i + 1);
				T tmp = list[j];
				list[j] = list[i];
				list[i] = tmp;
			}
		}

		// Token: 0x04000198 RID: 408
		private static readonly byte[] primes = new byte[] { 7, 11, 23, 37, 43, 59, 71 };

		// Token: 0x04000199 RID: 409
		private static readonly RNGCryptoServiceProvider _RNG = new RNGCryptoServiceProvider();

		// Token: 0x0400019A RID: 410
		private readonly SHA256Managed sha256 = new SHA256Managed();

		// Token: 0x0400019B RID: 411
		private int mixIndex;

		// Token: 0x0400019C RID: 412
		private byte[] state;

		// Token: 0x0400019D RID: 413
		private int stateFilled;

		// Token: 0x0400019E RID: 414
		private int seedLen;
	}
}
