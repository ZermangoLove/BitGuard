using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.VM;

namespace KoiVM.RT.Mutation
{
	// Token: 0x02000102 RID: 258
	internal class RuntimeMutator : IModuleWriterListener
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0001797C File Offset: 0x00015B7C
		public RuntimeMutator(ModuleDef module, VMRuntime rt)
		{
			this.RTModule = module;
			this.rt = rt;
			this.methodPatcher = new MethodPatcher(module);
			this.constants = new RTConstants();
			this.helpers = new RuntimeHelpers(this.constants, rt, module);
			this.constants.InjectConstants(module, rt.Descriptor, this.helpers);
			this.helpers.AddHelpers();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00003278 File Offset: 0x00001478
		public void InitHelpers()
		{
			this.helpers = new RuntimeHelpers(this.constants, this.rt, this.RTModule);
			this.helpers.AddHelpers();
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000032A4 File Offset: 0x000014A4
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x000032AC File Offset: 0x000014AC
		public ModuleDef RTModule { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x000032B5 File Offset: 0x000014B5
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x000032BD File Offset: 0x000014BD
		public byte[] RuntimeLib { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000032C6 File Offset: 0x000014C6
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x000032CE File Offset: 0x000014CE
		public byte[] RuntimeSym { get; private set; }

		// Token: 0x06000421 RID: 1057 RVA: 0x000179F0 File Offset: 0x00015BF0
		public void CommitRuntime(ModuleDef targetModule)
		{
			this.MutateRuntime();
			bool flag = targetModule == null;
			if (flag)
			{
				MemoryStream stream = new MemoryStream();
				MemoryStream pdbStream = new MemoryStream();
				ModuleWriterOptions options = new ModuleWriterOptions(this.RTModule);
				this.RTModule.Write(stream, options);
				this.RuntimeLib = stream.ToArray();
				this.RuntimeSym = new byte[0];
			}
			else
			{
				List<TypeDef> types = this.RTModule.Types.Where((TypeDef t) => !t.IsGlobalModuleType).ToList<TypeDef>();
				this.RTModule.Types.Clear();
				foreach (TypeDef type in types)
				{
					targetModule.Types.Add(type);
				}
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00017AEC File Offset: 0x00015CEC
		public IModuleWriterListener CommitModule(ModuleDef module)
		{
			this.ImportReferences(module);
			return this;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000032D7 File Offset: 0x000014D7
		public void ReplaceMethodStub(MethodDef method)
		{
			this.methodPatcher.PatchMethodStub(method, this.rt.Descriptor.Data.GetExportId(method));
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000424 RID: 1060 RVA: 0x00017B08 File Offset: 0x00015D08
		// (remove) Token: 0x06000425 RID: 1061 RVA: 0x00017B40 File Offset: 0x00015D40
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<RequestKoiEventArgs> RequestKoi;

		// Token: 0x06000426 RID: 1062 RVA: 0x00017B78 File Offset: 0x00015D78
		private void MutateRuntime()
		{
			IVMSettings settings = this.rt.Descriptor.Settings;
			RuntimePatcher.Patch(this.RTModule, settings.ExportDbgInfo, settings.DoStackWalk);
			this.constants.InjectConstants(this.RTModule, this.rt.Descriptor, this.helpers);
			new Renamer(this.rt.Descriptor.Random.Next()).Process(this.RTModule);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00017BF8 File Offset: 0x00015DF8
		private void ImportReferences(ModuleDef module)
		{
			List<KeyValuePair<IMemberRef, uint>> refCopy = this.rt.Descriptor.Data.refMap.ToList<KeyValuePair<IMemberRef, uint>>();
			this.rt.Descriptor.Data.refMap.Clear();
			foreach (KeyValuePair<IMemberRef, uint> mdRef in refCopy)
			{
				bool flag = mdRef.Key is ITypeDefOrRef;
				object item;
				if (flag)
				{
					item = module.Import((ITypeDefOrRef)mdRef.Key);
				}
				else
				{
					bool flag2 = mdRef.Key is MemberRef;
					if (flag2)
					{
						item = module.Import((MemberRef)mdRef.Key);
					}
					else
					{
						bool flag3 = mdRef.Key is MethodDef;
						if (flag3)
						{
							item = module.Import((MethodDef)mdRef.Key);
						}
						else
						{
							bool flag4 = mdRef.Key is MethodSpec;
							if (flag4)
							{
								item = module.Import((MethodSpec)mdRef.Key);
							}
							else
							{
								bool flag5 = mdRef.Key is FieldDef;
								if (flag5)
								{
									item = module.Import((FieldDef)mdRef.Key);
								}
								else
								{
									item = mdRef.Key;
								}
							}
						}
					}
				}
				this.rt.Descriptor.Data.refMap.Add((IMemberRef)item, mdRef.Value);
			}
			foreach (DataDescriptor.FuncSigDesc sig in this.rt.Descriptor.Data.sigs)
			{
				MethodSig methodSig = sig.Signature;
				FuncSig funcSig = sig.FuncSig;
				bool hasThis = methodSig.HasThis;
				if (hasThis)
				{
					FuncSig funcSig2 = funcSig;
					funcSig2.Flags |= this.rt.Descriptor.Runtime.RTFlags.INSTANCE;
				}
				List<ITypeDefOrRef> paramTypes = new List<ITypeDefOrRef>();
				bool flag6 = methodSig.HasThis && !methodSig.ExplicitThis;
				if (flag6)
				{
					bool isValueType = sig.DeclaringType.IsValueType;
					IType thisType;
					if (isValueType)
					{
						thisType = module.Import(new ByRefSig(sig.DeclaringType.ToTypeSig(true)).ToTypeDefOrRef());
					}
					else
					{
						thisType = module.Import(sig.DeclaringType);
					}
					paramTypes.Add((ITypeDefOrRef)thisType);
				}
				foreach (TypeSig param in methodSig.Params)
				{
					ITypeDefOrRef paramType = (ITypeDefOrRef)module.Import(param.ToTypeDefOrRef());
					paramTypes.Add(paramType);
				}
				funcSig.ParamSigs = paramTypes.ToArray();
				ITypeDefOrRef retType = (ITypeDefOrRef)module.Import(methodSig.RetType.ToTypeDefOrRef());
				funcSig.RetType = retType;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017F50 File Offset: 0x00016150
		private void MutateMetadata()
		{
			foreach (KeyValuePair<IMemberRef, uint> mdRef in this.rt.Descriptor.Data.refMap)
			{
				mdRef.Key.Rid = this.rtMD.GetToken(mdRef.Key).Rid;
			}
			foreach (DataDescriptor.FuncSigDesc sig in this.rt.Descriptor.Data.sigs)
			{
				FuncSig funcSig = sig.FuncSig;
				foreach (ITypeDefOrRef paramType in funcSig.ParamSigs)
				{
					paramType.Rid = this.rtMD.GetToken(paramType).Rid;
				}
				funcSig.RetType.Rid = this.rtMD.GetToken(funcSig.RetType).Rid;
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00018098 File Offset: 0x00016298
		void IModuleWriterListener.OnWriterEvent(ModuleWriterBase writer, ModuleWriterEvent evt)
		{
			this.rtWriter = writer;
			this.rtMD = writer.MetaData;
			bool flag = evt == ModuleWriterEvent.MDEndCreateTables;
			if (flag)
			{
				this.MutateMetadata();
				RequestKoiEventArgs request = new RequestKoiEventArgs();
				this.RequestKoi(this, request);
				writer.TheOptions.MetaDataOptions.OtherHeaps.Add(request.Heap);
				this.rt.ResetData();
			}
		}

		// Token: 0x040001C3 RID: 451
		private ModuleWriterBase rtWriter;

		// Token: 0x040001C4 RID: 452
		private MetaData rtMD;

		// Token: 0x040001C5 RID: 453
		private VMRuntime rt;

		// Token: 0x040001C6 RID: 454
		private RuntimeHelpers helpers;

		// Token: 0x040001C7 RID: 455
		private MethodPatcher methodPatcher;

		// Token: 0x040001C8 RID: 456
		internal RTConstants constants;
	}
}
