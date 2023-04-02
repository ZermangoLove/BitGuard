using System;
using KoiVM.CFG;
using KoiVM.RT;

namespace KoiVM.AST.IL
{
	// Token: 0x0200013E RID: 318
	public class ILJumpTable : IILOperand, IHasOffset
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00003B82 File Offset: 0x00001D82
		public ILJumpTable(IBasicBlock[] targets)
		{
			this.Targets = targets;
			this.Chunk = new JumpTableChunk(this);
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00003BA1 File Offset: 0x00001DA1
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00003BA9 File Offset: 0x00001DA9
		public JumpTableChunk Chunk { get; private set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00003BB2 File Offset: 0x00001DB2
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00003BBA File Offset: 0x00001DBA
		public ILInstruction RelativeBase { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00003BC3 File Offset: 0x00001DC3
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00003BCB File Offset: 0x00001DCB
		public IBasicBlock[] Targets { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001CE74 File Offset: 0x0001B074
		public uint Offset
		{
			get
			{
				return this.Chunk.Offset;
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001CE94 File Offset: 0x0001B094
		public override string ToString()
		{
			return string.Format("[..{0}..]", this.Targets.Length);
		}
	}
}
