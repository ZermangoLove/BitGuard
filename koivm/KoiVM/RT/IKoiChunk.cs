using System;

namespace KoiVM.RT
{
	// Token: 0x020000F1 RID: 241
	public interface IKoiChunk
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003A0 RID: 928
		uint Length { get; }

		// Token: 0x060003A1 RID: 929
		void OnOffsetComputed(uint offset);

		// Token: 0x060003A2 RID: 930
		byte[] GetData();
	}
}
