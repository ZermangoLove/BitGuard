using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.AST.ILAST;
using KoiVM.CFG;
using KoiVM.ILAST.Transformation;
using KoiVM.RT;
using KoiVM.VM;

namespace KoiVM.ILAST
{
	// Token: 0x02000110 RID: 272
	public class ILASTTransformer
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x0000348A File Offset: 0x0000168A
		public ILASTTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
		{
			this.RootScope = rootScope;
			this.Method = method;
			this.Runtime = runtime;
			this.Annotations = new Dictionary<object, object>();
			this.InitPipeline();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00019528 File Offset: 0x00017728
		private void InitPipeline()
		{
			this.pipeline = new ITransformationHandler[]
			{
				new VariableInlining(),
				new StringTransform(),
				new ArrayTransform(),
				new IndirectTransform(),
				new ILASTTypeInference(),
				new NullTransform(),
				new BranchTransform()
			};
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000034BF File Offset: 0x000016BF
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x000034C7 File Offset: 0x000016C7
		public MethodDef Method { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000034D0 File Offset: 0x000016D0
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x000034D8 File Offset: 0x000016D8
		public ScopeBlock RootScope { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x000034E1 File Offset: 0x000016E1
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x000034E9 File Offset: 0x000016E9
		public VMRuntime Runtime { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0001957C File Offset: 0x0001777C
		public VMDescriptor VM
		{
			get
			{
				return this.Runtime.Descriptor;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000034F2 File Offset: 0x000016F2
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x000034FA File Offset: 0x000016FA
		internal Dictionary<object, object> Annotations { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00003503 File Offset: 0x00001703
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x0000350B File Offset: 0x0000170B
		internal BasicBlock<ILASTTree> Block { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001959C File Offset: 0x0001779C
		internal ILASTTree Tree
		{
			get
			{
				return this.Block.Content;
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000195BC File Offset: 0x000177BC
		public void Transform()
		{
			bool flag = this.pipeline == null;
			if (flag)
			{
				throw new InvalidOperationException("Transformer already used.");
			}
			ITransformationHandler[] array = this.pipeline;
			for (int i = 0; i < array.Length; i++)
			{
				ITransformationHandler handler = array[i];
				handler.Initialize(this);
				this.RootScope.ProcessBasicBlocks<ILASTTree>(delegate(BasicBlock<ILASTTree> block)
				{
					this.Block = block;
					handler.Transform(this);
				});
			}
			this.pipeline = null;
		}

		// Token: 0x040001EF RID: 495
		private ITransformationHandler[] pipeline;
	}
}
