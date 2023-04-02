using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.AST.IL;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VMIL.Transforms;

namespace KoiVM.VMIL
{
	// Token: 0x020000B1 RID: 177
	public class ILPostTransformer
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000028FD File Offset: 0x00000AFD
		public ILPostTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
		{
			this.RootScope = rootScope;
			this.Method = method;
			this.Runtime = runtime;
			this.Annotations = new Dictionary<object, object>();
			this.pipeline = this.InitPipeline();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000FAF4 File Offset: 0x0000DCF4
		private IPostTransform[] InitPipeline()
		{
			return new IPostTransform[]
			{
				new SaveRegistersTransform(),
				new FixMethodRefTransform(),
				new BlockKeyTransform()
			};
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00002937 File Offset: 0x00000B37
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000293F File Offset: 0x00000B3F
		public VMRuntime Runtime { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00002948 File Offset: 0x00000B48
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00002950 File Offset: 0x00000B50
		public MethodDef Method { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00002959 File Offset: 0x00000B59
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00002961 File Offset: 0x00000B61
		public ScopeBlock RootScope { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000296A File Offset: 0x00000B6A
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00002972 File Offset: 0x00000B72
		internal Dictionary<object, object> Annotations { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000297B File Offset: 0x00000B7B
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x00002983 File Offset: 0x00000B83
		internal ILBlock Block { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000FB24 File Offset: 0x0000DD24
		internal ILInstrList Instructions
		{
			get
			{
				return this.Block.Content;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public void Transform()
		{
			bool flag = this.pipeline == null;
			if (flag)
			{
				throw new InvalidOperationException("Transformer already used.");
			}
			IPostTransform[] array = this.pipeline;
			for (int i = 0; i < array.Length; i++)
			{
				IPostTransform handler = array[i];
				handler.Initialize(this);
				this.RootScope.ProcessBasicBlocks<ILInstrList>(delegate(BasicBlock<ILInstrList> block)
				{
					this.Block = (ILBlock)block;
					handler.Transform(this);
				});
			}
			this.pipeline = null;
		}

		// Token: 0x040000F2 RID: 242
		private IPostTransform[] pipeline;
	}
}
