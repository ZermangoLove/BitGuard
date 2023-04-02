using System;
using System.Collections.Generic;
using System.IO;
using dnlib.DotNet.Writer;

namespace KoiVM.RT
{
	// Token: 0x020000F3 RID: 243
	internal class KoiHeap : HeapBase
	{
		// Token: 0x060003AB RID: 939 RVA: 0x000137D4 File Offset: 0x000119D4
		public uint AddChunk(byte[] chunk)
		{
			uint offset = this.currentLen;
			this.chunks.Add(chunk);
			this.currentLen += (uint)chunk.Length;
			return offset;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001380C File Offset: 0x00011A0C
		public override string Name
		{
			get
			{
				return "?";
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00013824 File Offset: 0x00011A24
		public override uint GetRawLength()
		{
			return this.currentLen;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001383C File Offset: 0x00011A3C
		protected override void WriteToImpl(BinaryWriter writer)
		{
			foreach (byte[] chunk in this.chunks)
			{
				writer.Write(chunk);
			}
		}

		// Token: 0x04000183 RID: 387
		private List<byte[]> chunks = new List<byte[]>();

		// Token: 0x04000184 RID: 388
		private uint currentLen;
	}
}
