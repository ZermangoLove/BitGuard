using System;
using KoiVM.AST.IL;
using KoiVM.RT;

namespace KoiVM.Protections.SMC
{
	// Token: 0x02000108 RID: 264
	internal class SMCBlockRef : ILRelReference
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x000033DD File Offset: 0x000015DD
		public SMCBlockRef(IHasOffset target, IHasOffset relBase, uint key)
			: base(target, relBase)
		{
			this.Key = key;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x000033F1 File Offset: 0x000015F1
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x000033F9 File Offset: 0x000015F9
		public uint Key { get; set; }

		// Token: 0x06000441 RID: 1089 RVA: 0x00018590 File Offset: 0x00016790
		public override uint Resolve(VMRuntime runtime)
		{
			return base.Resolve(runtime) ^ this.Key;
		}
	}
}
