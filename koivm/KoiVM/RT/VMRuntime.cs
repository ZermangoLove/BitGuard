using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.CFG;
using KoiVM.RT.Mutation;
using KoiVM.VM;

namespace KoiVM.RT
{
	// Token: 0x020000F4 RID: 244
	public class VMRuntime
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00003013 File Offset: 0x00001213
		public VMRuntime(IVMSettings settings, ModuleDef rt)
		{
			this.settings = settings;
			this.Init(rt);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00013894 File Offset: 0x00011A94
		[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
		private void Init(ModuleDef rt)
		{
			this.Descriptor = new VMDescriptor(this.settings);
			this.methodMap = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
			this.basicBlocks = new List<Tuple<MethodDef, ILBlock>>();
			this.extraChunks = new List<IKoiChunk>();
			this.finalChunks = new List<IKoiChunk>();
			this.serializer = new BasicBlockSerializer(this);
			this.rtMutator = new RuntimeMutator(rt, this);
			this.rtMutator.RequestKoi += this.OnKoiRequested;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00013914 File Offset: 0x00011B14
		public ModuleDef Module
		{
			get
			{
				return this.rtMutator.RTModule;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000302C File Offset: 0x0000122C
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00003034 File Offset: 0x00001234
		public VMDescriptor Descriptor { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000303D File Offset: 0x0000123D
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00003045 File Offset: 0x00001245
		public byte[] RuntimeLibrary { get; private set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000304E File Offset: 0x0000124E
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00003056 File Offset: 0x00001256
		public byte[] RuntimeSymbols { get; private set; }

		// Token: 0x060003B9 RID: 953 RVA: 0x00013934 File Offset: 0x00011B34
		public void AddMethod(MethodDef method, ScopeBlock rootScope)
		{
			ILBlock entry = null;
			foreach (IBasicBlock basicBlock in rootScope.GetBasicBlocks())
			{
				ILBlock block = (ILBlock)basicBlock;
				bool flag = block.Id == 0;
				if (flag)
				{
					entry = block;
				}
				this.basicBlocks.Add(Tuple.Create<MethodDef, ILBlock>(method, block));
			}
			Debug.Assert(entry != null);
			this.methodMap[method] = Tuple.Create<ScopeBlock, ILBlock>(rootScope, entry);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000305F File Offset: 0x0000125F
		internal void AddHelper(MethodDef method, ScopeBlock rootScope, ILBlock entry)
		{
			this.methodMap[method] = Tuple.Create<ScopeBlock, ILBlock>(rootScope, entry);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00003076 File Offset: 0x00001276
		public void AddBlock(MethodDef method, ILBlock block)
		{
			this.basicBlocks.Add(Tuple.Create<MethodDef, ILBlock>(method, block));
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000139C8 File Offset: 0x00011BC8
		public ScopeBlock LookupMethod(MethodDef method)
		{
			Tuple<ScopeBlock, ILBlock> i = this.methodMap[method];
			return i.Item1;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000139F0 File Offset: 0x00011BF0
		public ScopeBlock LookupMethod(MethodDef method, out ILBlock entry)
		{
			Tuple<ScopeBlock, ILBlock> i = this.methodMap[method];
			entry = i.Item2;
			return i.Item1;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000308C File Offset: 0x0000128C
		public void AddChunk(IKoiChunk chunk)
		{
			this.extraChunks.Add(chunk);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000309C File Offset: 0x0000129C
		public void ExportMethod(MethodDef method)
		{
			this.rtMutator.ReplaceMethodStub(method);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00013A20 File Offset: 0x00011C20
		public IModuleWriterListener CommitModule(ModuleDefMD module)
		{
			return this.rtMutator.CommitModule(module);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000030AC File Offset: 0x000012AC
		public void CommitRuntime(ModuleDef targetModule = null)
		{
			this.rtMutator.CommitRuntime(targetModule);
			this.RuntimeLibrary = this.rtMutator.RuntimeLib;
			this.RuntimeSymbols = this.rtMutator.RuntimeSym;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00013A40 File Offset: 0x00011C40
		[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
		private void OnKoiRequested(object sender, RequestKoiEventArgs e)
		{
			HeaderChunk header = new HeaderChunk(this);
			foreach (Tuple<MethodDef, ILBlock> block in this.basicBlocks)
			{
				this.finalChunks.Add(block.Item2.CreateChunk(this, block.Item1));
			}
			this.finalChunks.AddRange(this.extraChunks);
			this.finalChunks.Add(new BinaryChunk(Watermark.GenerateWatermark((uint)this.settings.Seed)));
			this.Descriptor.Random.Shuffle(this.finalChunks);
			this.finalChunks.Insert(0, header);
			this.ComputeOffsets();
			this.FixupReferences();
			header.WriteData(this);
			e.Heap = this.CreateHeap();
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00013B34 File Offset: 0x00011D34
		private void ComputeOffsets()
		{
			uint offset = 0U;
			foreach (IKoiChunk chunk in this.finalChunks)
			{
				chunk.OnOffsetComputed(offset);
				offset += chunk.Length;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013B98 File Offset: 0x00011D98
		private void FixupReferences()
		{
			foreach (Tuple<MethodDef, ILBlock> block in this.basicBlocks)
			{
				foreach (ILInstruction instr in block.Item2.Content)
				{
					bool flag = instr.Operand is ILRelReference;
					if (flag)
					{
						ILRelReference reference = (ILRelReference)instr.Operand;
						instr.Operand = ILImmediate.Create(reference.Resolve(this), ASTType.I4);
					}
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00013C70 File Offset: 0x00011E70
		private KoiHeap CreateHeap()
		{
			bool exportDbgInfo = this.settings.ExportDbgInfo;
			if (exportDbgInfo)
			{
				this.dbgWriter = new DbgWriter();
			}
			KoiHeap heap = new KoiHeap();
			foreach (IKoiChunk chunk in this.finalChunks)
			{
				heap.AddChunk(chunk.GetData());
			}
			bool flag = this.dbgWriter != null;
			if (flag)
			{
				using (DbgWriter.DbgSerializer serializer = this.dbgWriter.GetSerializer())
				{
					foreach (IKoiChunk chunk2 in this.finalChunks)
					{
						serializer.WriteBlock(chunk2 as BasicBlockChunk);
					}
				}
			}
			return heap;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00013D80 File Offset: 0x00011F80
		public byte[] DebugInfo
		{
			get
			{
				return this.dbgWriter.GetDbgInfo();
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00013DA0 File Offset: 0x00011FA0
		public void ResetData()
		{
			this.methodMap = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
			this.basicBlocks = new List<Tuple<MethodDef, ILBlock>>();
			this.extraChunks = new List<IKoiChunk>();
			this.finalChunks = new List<IKoiChunk>();
			this.Descriptor.ResetData();
			this.rtMutator.InitHelpers();
		}

		// Token: 0x04000185 RID: 389
		internal Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>> methodMap;

		// Token: 0x04000186 RID: 390
		private List<Tuple<MethodDef, ILBlock>> basicBlocks;

		// Token: 0x04000187 RID: 391
		private List<IKoiChunk> extraChunks;

		// Token: 0x04000188 RID: 392
		private List<IKoiChunk> finalChunks;

		// Token: 0x04000189 RID: 393
		internal BasicBlockSerializer serializer;

		// Token: 0x0400018A RID: 394
		private IVMSettings settings;

		// Token: 0x0400018B RID: 395
		private RuntimeMutator rtMutator;

		// Token: 0x0400018F RID: 399
		internal DbgWriter dbgWriter;
	}
}
