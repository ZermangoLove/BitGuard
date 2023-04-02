using System;
using System.Collections.Generic;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VM;
using KoiVM.VMIR.Transforms;

namespace KoiVM.VMIR
{
	// Token: 0x02000023 RID: 35
	public class IRTransformer
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000253A File Offset: 0x0000073A
		public IRTransformer(ScopeBlock rootScope, IRContext ctx, VMRuntime runtime)
		{
			this.RootScope = rootScope;
			this.Context = ctx;
			this.Runtime = runtime;
			this.Annotations = new Dictionary<object, object>();
			this.InitPipeline();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006648 File Offset: 0x00004848
		private void InitPipeline()
		{
			this.pipeline = new ITransform[]
			{
				this.Context.IsRuntime ? null : new GuardBlockTransform(),
				this.Context.IsRuntime ? null : new EHTransform(),
				new InitLocalTransform(),
				new ConstantTypePromotionTransform(),
				new GetSetFlagTransform(),
				new LogicTransform(),
				new InvokeTransform(),
				new MetadataTransform(),
				this.Context.IsRuntime ? null : new RegisterAllocationTransform(),
				this.Context.IsRuntime ? null : new StackFrameTransform(),
				new LeaTransform(),
				this.Context.IsRuntime ? null : new MarkReturnRegTransform()
			};
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000256F File Offset: 0x0000076F
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00002577 File Offset: 0x00000777
		public IRContext Context { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00002580 File Offset: 0x00000780
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00002588 File Offset: 0x00000788
		public VMRuntime Runtime { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00006718 File Offset: 0x00004918
		public VMDescriptor VM
		{
			get
			{
				return this.Runtime.Descriptor;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00002591 File Offset: 0x00000791
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00002599 File Offset: 0x00000799
		public ScopeBlock RootScope { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000025A2 File Offset: 0x000007A2
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000025AA File Offset: 0x000007AA
		internal Dictionary<object, object> Annotations { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000025B3 File Offset: 0x000007B3
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000025BB File Offset: 0x000007BB
		internal BasicBlock<IRInstrList> Block { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006738 File Offset: 0x00004938
		internal IRInstrList Instructions
		{
			get
			{
				return this.Block.Content;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006758 File Offset: 0x00004958
		public void Transform()
		{
			bool flag = this.pipeline == null;
			if (flag)
			{
				throw new InvalidOperationException("Transformer already used.");
			}
			ITransform[] array = this.pipeline;
			for (int i = 0; i < array.Length; i++)
			{
				ITransform handler = array[i];
				bool flag2 = handler == null;
				if (!flag2)
				{
					handler.Initialize(this);
					this.RootScope.ProcessBasicBlocks<IRInstrList>(delegate(BasicBlock<IRInstrList> block)
					{
						this.Block = block;
						handler.Transform(this);
					});
				}
			}
			this.pipeline = null;
		}

		// Token: 0x0400008A RID: 138
		private ITransform[] pipeline;
	}
}
