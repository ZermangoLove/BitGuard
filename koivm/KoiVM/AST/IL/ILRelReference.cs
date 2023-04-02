using System;
using KoiVM.RT;

namespace KoiVM.AST.IL
{
	// Token: 0x02000148 RID: 328
	public class ILRelReference : IILOperand
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x00003D74 File Offset: 0x00001F74
		public ILRelReference(IHasOffset target, IHasOffset relBase)
		{
			this.Target = target;
			this.Base = relBase;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00003D8E File Offset: 0x00001F8E
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00003D96 File Offset: 0x00001F96
		public IHasOffset Target { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00003D9F File Offset: 0x00001F9F
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00003DA7 File Offset: 0x00001FA7
		public IHasOffset Base { get; set; }

		// Token: 0x0600059E RID: 1438 RVA: 0x0001D1CC File Offset: 0x0001B3CC
		public virtual uint Resolve(VMRuntime runtime)
		{
			uint relBase = this.Base.Offset;
			bool flag = this.Base is ILInstruction;
			if (flag)
			{
				relBase += runtime.serializer.ComputeLength((ILInstruction)this.Base);
			}
			return this.Target.Offset - relBase;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001D224 File Offset: 0x0001B424
		public override string ToString()
		{
			return string.Format("[{0:x8}:{1:x8}]", this.Base.Offset, this.Target.Offset);
		}
	}
}
