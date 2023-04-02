using System;
using System.Reflection;
using dnlib.DotNet;
using KoiVM.CFG;
using KoiVM.ILAST;
using KoiVM.RT;
using KoiVM.VMIL;
using KoiVM.VMIR;

namespace KoiVM
{
	// Token: 0x02000005 RID: 5
	[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
	public class MethodVirtualizer
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000020DC File Offset: 0x000002DC
		public MethodVirtualizer(VMRuntime runtime)
		{
			this.Runtime = runtime;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000020EE File Offset: 0x000002EE
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000020F6 File Offset: 0x000002F6
		private protected VMRuntime Runtime { protected get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000020FF File Offset: 0x000002FF
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002107 File Offset: 0x00000307
		private protected MethodDef Method { protected get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002110 File Offset: 0x00000310
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002118 File Offset: 0x00000318
		private protected ScopeBlock RootScope { protected get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002121 File Offset: 0x00000321
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002129 File Offset: 0x00000329
		private protected IRContext IRContext { protected get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002132 File Offset: 0x00000332
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000213A File Offset: 0x0000033A
		private protected bool IsExport { protected get; private set; }

		// Token: 0x06000021 RID: 33 RVA: 0x00004858 File Offset: 0x00002A58
		public ScopeBlock Run(MethodDef method, bool isExport)
		{
			ScopeBlock scopeBlock;
			try
			{
				this.Method = method;
				this.IsExport = isExport;
				this.Init();
				this.BuildILAST();
				this.TransformILAST();
				this.BuildVMIR();
				this.TransformVMIR();
				this.BuildVMIL();
				this.TransformVMIL();
				this.Deinitialize();
				ScopeBlock scope = this.RootScope;
				this.RootScope = null;
				this.Method = null;
				scopeBlock = scope;
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Failed to translate method {0}.", method), ex);
			}
			return scopeBlock;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002143 File Offset: 0x00000343
		protected virtual void Init()
		{
			this.RootScope = BlockParser.Parse(this.Method, this.Method.Body);
			this.IRContext = new IRContext(this.Method, this.Method.Body);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002180 File Offset: 0x00000380
		protected virtual void BuildILAST()
		{
			ILASTBuilder.BuildAST(this.Method, this.Method.Body, this.RootScope);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000048F0 File Offset: 0x00002AF0
		protected virtual void TransformILAST()
		{
			ILASTTransformer transformer = new ILASTTransformer(this.Method, this.RootScope, this.Runtime);
			transformer.Transform();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004920 File Offset: 0x00002B20
		protected virtual void BuildVMIR()
		{
			IRTranslator translator = new IRTranslator(this.IRContext, this.Runtime);
			translator.Translate(this.RootScope);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004950 File Offset: 0x00002B50
		protected virtual void TransformVMIR()
		{
			IRTransformer transformer = new IRTransformer(this.RootScope, this.IRContext, this.Runtime);
			transformer.Transform();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004980 File Offset: 0x00002B80
		protected virtual void BuildVMIL()
		{
			ILTranslator translator = new ILTranslator(this.Runtime);
			translator.Translate(this.RootScope);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000049A8 File Offset: 0x00002BA8
		protected virtual void TransformVMIL()
		{
			ILTransformer transformer = new ILTransformer(this.Method, this.RootScope, this.Runtime);
			transformer.Transform();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000049D8 File Offset: 0x00002BD8
		protected virtual void Deinitialize()
		{
			this.IRContext = null;
			this.Runtime.AddMethod(this.Method, this.RootScope);
			bool isExport = this.IsExport;
			if (isExport)
			{
				this.Runtime.ExportMethod(this.Method);
			}
		}
	}
}
