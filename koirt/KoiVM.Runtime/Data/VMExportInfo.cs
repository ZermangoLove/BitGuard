using System;
using System.Reflection;

namespace KoiVM.Runtime.Data
{
	// Token: 0x02000077 RID: 119
	internal struct VMExportInfo
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x0000C244 File Offset: 0x0000A444
		public unsafe VMExportInfo(ref byte* ptr, Module module)
		{
			this.CodeOffset = *ptr;
			ptr += 4;
			bool flag = this.CodeOffset > 0U;
			if (flag)
			{
				this.EntryKey = *ptr;
				ptr += 4;
			}
			else
			{
				this.EntryKey = 0U;
			}
			this.Signature = new VMFuncSig(ref ptr, module);
		}

		// Token: 0x040000CF RID: 207
		public readonly uint CodeOffset;

		// Token: 0x040000D0 RID: 208
		public readonly uint EntryKey;

		// Token: 0x040000D1 RID: 209
		public readonly VMFuncSig Signature;
	}
}
