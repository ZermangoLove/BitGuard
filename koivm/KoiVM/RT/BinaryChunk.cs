using System;

namespace KoiVM.RT
{
	// Token: 0x020000EA RID: 234
	public class BinaryChunk : IKoiChunk
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00002EB7 File Offset: 0x000010B7
		public BinaryChunk(byte[] data)
		{
			this.Data = data;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00002EC9 File Offset: 0x000010C9
		// (set) Token: 0x0600037D RID: 893 RVA: 0x00002ED1 File Offset: 0x000010D1
		public byte[] Data { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00002EDA File Offset: 0x000010DA
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00002EE2 File Offset: 0x000010E2
		public uint Offset { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00012914 File Offset: 0x00010B14
		uint IKoiChunk.Length
		{
			get
			{
				return (uint)this.Data.Length;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00012930 File Offset: 0x00010B30
		void IKoiChunk.OnOffsetComputed(uint offset)
		{
			bool flag = this.OffsetComputed != null;
			if (flag)
			{
				this.OffsetComputed(this, new OffsetComputeEventArgs(offset));
			}
			this.Offset = offset;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00012968 File Offset: 0x00010B68
		byte[] IKoiChunk.GetData()
		{
			return this.Data;
		}

		// Token: 0x0400016D RID: 365
		public EventHandler<OffsetComputeEventArgs> OffsetComputed;
	}
}
