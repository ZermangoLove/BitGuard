using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.VM;
using KoiVM.VMIL;
using KoiVM.VMIR;

namespace KoiVM.RT.Mutation
{
	// Token: 0x02000100 RID: 256
	public class RuntimeHelpers
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x00017684 File Offset: 0x00015884
		public RuntimeHelpers(RTConstants constants, VMRuntime rt, ModuleDef rtModule)
		{
			this.rt = rt;
			this.rtModule = rtModule;
			this.constants = constants;
			this.rtHelperType = new TypeDefUser("KoiVM.Runtime", "Helpers");
			this.AllocateHelpers();
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00003231 File Offset: 0x00001431
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00003239 File Offset: 0x00001439
		public uint INIT { get; private set; }

		// Token: 0x06000412 RID: 1042 RVA: 0x000176D4 File Offset: 0x000158D4
		private MethodDef CreateHelperMethod(string name)
		{
			return new MethodDefUser(name, MethodSig.CreateStatic(this.rtModule.CorLibTypes.Void))
			{
				Body = new CilBody()
			};
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00003242 File Offset: 0x00001442
		private void AllocateHelpers()
		{
			this.methodINIT = this.CreateHelperMethod("INIT");
			this.INIT = this.rt.Descriptor.Data.GetExportId(this.methodINIT);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00017714 File Offset: 0x00015914
		public void AddHelpers()
		{
			ScopeBlock scope = new ScopeBlock();
			BasicBlock<IRInstrList> initBlock = new BasicBlock<IRInstrList>(1, new IRInstrList
			{
				new IRInstruction(IROpCode.RET)
			});
			scope.Content.Add(initBlock);
			BasicBlock<IRInstrList> retnBlock = new BasicBlock<IRInstrList>(0, new IRInstrList
			{
				new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(this.rt.Descriptor.Runtime.VMCall[VMCalls.EXIT]))
			});
			scope.Content.Add(initBlock);
			this.CompileHelpers(this.methodINIT, scope);
			VMMethodInfo info = this.rt.Descriptor.Data.LookupInfo(this.methodINIT);
			scope.ProcessBasicBlocks<ILInstrList>(delegate(BasicBlock<ILInstrList> block)
			{
				bool flag = block.Id == 1;
				if (flag)
				{
					this.AddHelper(null, this.methodINIT, (ILBlock)block);
					VMBlockKey blockKey = info.BlockKeys[block];
					info.EntryKey = blockKey.EntryKey;
					info.ExitKey = blockKey.ExitKey;
				}
				this.rt.AddBlock(this.methodINIT, (ILBlock)block);
			});
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000177E4 File Offset: 0x000159E4
		private void AddHelper(VMMethodInfo info, MethodDef method, ILBlock block)
		{
			ScopeBlock helperScope = new ScopeBlock();
			block.Id = 0;
			helperScope.Content.Add(block);
			bool flag = info != null;
			if (flag)
			{
				VMMethodInfo helperInfo = new VMMethodInfo();
				VMBlockKey keys = info.BlockKeys[block];
				helperInfo.RootScope = helperScope;
				helperInfo.EntryKey = keys.EntryKey;
				helperInfo.ExitKey = keys.ExitKey;
				this.rt.Descriptor.Data.SetInfo(method, helperInfo);
			}
			this.rt.AddHelper(method, helperScope, block);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00017870 File Offset: 0x00015A70
		private void CompileHelpers(MethodDef method, ScopeBlock scope)
		{
			IRTransformer irTransformer = new IRTransformer(scope, new IRContext(method, method.Body)
			{
				IsRuntime = true
			}, this.rt);
			irTransformer.Transform();
			ILTranslator ilTranslator = new ILTranslator(this.rt);
			ILTransformer ilTransformer = new ILTransformer(method, scope, this.rt);
			ilTranslator.Translate(scope);
			ilTransformer.Transform();
			ILPostTransformer postTransformer = new ILPostTransformer(method, scope, this.rt);
			postTransformer.Transform();
		}

		// Token: 0x040001BB RID: 443
		private VMRuntime rt;

		// Token: 0x040001BC RID: 444
		private TypeDef rtHelperType;

		// Token: 0x040001BD RID: 445
		private ModuleDef rtModule;

		// Token: 0x040001BE RID: 446
		private RTConstants constants;

		// Token: 0x040001BF RID: 447
		private MethodDef methodINIT;
	}
}
