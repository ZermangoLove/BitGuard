using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.AST.IL;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VM;
using KoiVM.VMIL.Transforms;

namespace KoiVM.VMIL
{
	// Token: 0x020000B7 RID: 183
	public class ILTransformer
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x000029FC File Offset: 0x00000BFC
		public ILTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
		{
			this.RootScope = rootScope;
			this.Method = method;
			this.Runtime = runtime;
			this.Annotations = new Dictionary<object, object>();
			this.pipeline = this.InitPipeline();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000FF08 File Offset: 0x0000E108
		private ITransform[] InitPipeline()
		{
			return new ITransform[]
			{
				new ReferenceOffsetTransform(),
				new EntryExitTransform(),
				new SaveInfoTransform()
			};
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00002A36 File Offset: 0x00000C36
		// (set) Token: 0x060002CA RID: 714 RVA: 0x00002A3E File Offset: 0x00000C3E
		public VMRuntime Runtime { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00002A47 File Offset: 0x00000C47
		// (set) Token: 0x060002CC RID: 716 RVA: 0x00002A4F File Offset: 0x00000C4F
		public MethodDef Method { get; private set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00002A58 File Offset: 0x00000C58
		// (set) Token: 0x060002CE RID: 718 RVA: 0x00002A60 File Offset: 0x00000C60
		public ScopeBlock RootScope { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000FF38 File Offset: 0x0000E138
		public VMDescriptor VM
		{
			get
			{
				return this.Runtime.Descriptor;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00002A69 File Offset: 0x00000C69
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00002A71 File Offset: 0x00000C71
		internal Dictionary<object, object> Annotations { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00002A7A File Offset: 0x00000C7A
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00002A82 File Offset: 0x00000C82
		internal ILBlock Block { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000FF58 File Offset: 0x0000E158
		internal ILInstrList Instructions
		{
			get
			{
				return this.Block.Content;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000FF78 File Offset: 0x0000E178
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
				handler.Initialize(this);
				this.RootScope.ProcessBasicBlocks<ILInstrList>(delegate(BasicBlock<ILInstrList> block)
				{
					this.Block = (ILBlock)block;
					handler.Transform(this);
				});
			}
			this.pipeline = null;
		}

		// Token: 0x0400014B RID: 331
		private ITransform[] pipeline;
	}
}
