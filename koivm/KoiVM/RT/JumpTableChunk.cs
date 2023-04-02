using System;
using KoiVM.AST.IL;

namespace KoiVM.RT
{
	// Token: 0x020000F2 RID: 242
	public class JumpTableChunk : IKoiChunk
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x0001365C File Offset: 0x0001185C
		public JumpTableChunk(ILJumpTable table)
		{
			this.Table = table;
			bool flag = table.Targets.Length > 65535;
			if (flag)
			{
				throw new NotSupportedException("Jump table too large.");
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00002FD0 File Offset: 0x000011D0
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00002FD8 File Offset: 0x000011D8
		public ILJumpTable Table { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00002FE1 File Offset: 0x000011E1
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00002FE9 File Offset: 0x000011E9
		public uint Offset { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00013698 File Offset: 0x00011898
		uint IKoiChunk.Length
		{
			get
			{
				return (uint)(this.Table.Targets.Length * 4 + 2);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00002FF2 File Offset: 0x000011F2
		void IKoiChunk.OnOffsetComputed(uint offset)
		{
			this.Offset = offset + 2U;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000136BC File Offset: 0x000118BC
		byte[] IKoiChunk.GetData()
		{
			byte[] data = new byte[this.Table.Targets.Length * 4 + 2];
			ushort len = (ushort)this.Table.Targets.Length;
			int ptr = 0;
			data[ptr++] = (byte)this.Table.Targets.Length;
			data[ptr++] = (byte)(this.Table.Targets.Length >> 8);
			uint relBase = this.Table.RelativeBase.Offset;
			relBase += this.runtime.serializer.ComputeLength(this.Table.RelativeBase);
			for (int i = 0; i < this.Table.Targets.Length; i++)
			{
				uint offset = ((ILBlock)this.Table.Targets[i]).Content[0].Offset;
				offset -= relBase;
				data[ptr++] = (byte)offset;
				data[ptr++] = (byte)(offset >> 8);
				data[ptr++] = (byte)(offset >> 16);
				data[ptr++] = (byte)(offset >> 24);
			}
			return data;
		}

		// Token: 0x04000180 RID: 384
		internal VMRuntime runtime;
	}
}
