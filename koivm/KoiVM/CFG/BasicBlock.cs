using System;
using System.Collections.Generic;

namespace KoiVM.CFG
{
	// Token: 0x0200011D RID: 285
	public class BasicBlock<TContent> : IBasicBlock
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00003603 File Offset: 0x00001803
		public BasicBlock(int id, TContent content)
		{
			this.Id = id;
			this.Content = content;
			this.Sources = new List<BasicBlock<TContent>>();
			this.Targets = new List<BasicBlock<TContent>>();
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00003635 File Offset: 0x00001835
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0000363D File Offset: 0x0000183D
		public int Id { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00003646 File Offset: 0x00001846
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0000364E File Offset: 0x0000184E
		public TContent Content { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00003657 File Offset: 0x00001857
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000365F File Offset: 0x0000185F
		public BlockFlags Flags { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00003668 File Offset: 0x00001868
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00003670 File Offset: 0x00001870
		public IList<BasicBlock<TContent>> Sources { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00003679 File Offset: 0x00001879
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00003681 File Offset: 0x00001881
		public IList<BasicBlock<TContent>> Targets { get; private set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001B1A0 File Offset: 0x000193A0
		object IBasicBlock.Content
		{
			get
			{
				return this.Content;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001B1C0 File Offset: 0x000193C0
		IEnumerable<IBasicBlock> IBasicBlock.Sources
		{
			get
			{
				return this.Sources;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001B1D8 File Offset: 0x000193D8
		IEnumerable<IBasicBlock> IBasicBlock.Targets
		{
			get
			{
				return this.Targets;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000368A File Offset: 0x0000188A
		public void LinkTo(BasicBlock<TContent> target)
		{
			this.Targets.Add(target);
			target.Sources.Add(this);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001B1F0 File Offset: 0x000193F0
		public override string ToString()
		{
			return string.Format("Block_{0:x2}:{1}{2}", this.Id, Environment.NewLine, this.Content);
		}
	}
}
