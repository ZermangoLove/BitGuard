using System;

namespace KoiVM.RT.Mutation
{
	// Token: 0x02000104 RID: 260
	internal class RequestKoiEventArgs : EventArgs
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00003314 File Offset: 0x00001514
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0000331C File Offset: 0x0000151C
		public KoiHeap Heap { get; set; }
	}
}
