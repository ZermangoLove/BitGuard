using System;
using dnlib.DotNet;
using KoiVM.RT;

namespace KoiVM.Protections.SMC
{
	// Token: 0x02000107 RID: 263
	internal class SMCBlockChunk : BasicBlockChunk, IKoiChunk
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x000033AA File Offset: 0x000015AA
		public SMCBlockChunk(VMRuntime rt, MethodDef method, SMCBlock block)
			: base(rt, method, block)
		{
			block.CounterOperand.Value = base.Length + 1U;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00018518 File Offset: 0x00016718
		uint IKoiChunk.Length
		{
			get
			{
				return base.Length + 1U;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000033D0 File Offset: 0x000015D0
		void IKoiChunk.OnOffsetComputed(uint offset)
		{
			base.OnOffsetComputed(offset + 1U);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00018534 File Offset: 0x00016734
		byte[] IKoiChunk.GetData()
		{
			byte[] data = base.GetData();
			byte[] newData = new byte[data.Length + 1];
			byte key = ((SMCBlock)base.Block).Key;
			for (int i = 0; i < data.Length; i++)
			{
				newData[i + 1] = data[i] ^ key;
			}
			newData[0] = key;
			return newData;
		}
	}
}
