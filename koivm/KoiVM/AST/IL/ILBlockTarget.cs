using System;
using KoiVM.CFG;

namespace KoiVM.AST.IL
{
	// Token: 0x02000147 RID: 327
	public class ILBlockTarget : IILOperand, IHasOffset
	{
		// Token: 0x06000594 RID: 1428 RVA: 0x00003D51 File Offset: 0x00001F51
		public ILBlockTarget(IBasicBlock target)
		{
			this.Target = target;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00003D63 File Offset: 0x00001F63
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00003D6B File Offset: 0x00001F6B
		public IBasicBlock Target { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001D170 File Offset: 0x0001B370
		public uint Offset
		{
			get
			{
				return ((ILBlock)this.Target).Content[0].Offset;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001D1A0 File Offset: 0x0001B3A0
		public override string ToString()
		{
			return string.Format("Block_{0:x2}", this.Target.Id);
		}
	}
}
