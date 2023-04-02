using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VMIL;

namespace KoiVM
{
	// Token: 0x02000008 RID: 8
	[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
	public class Virtualizer : IVMSettings
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00005160 File Offset: 0x00003360
		public ModuleDef RuntimeModule
		{
			get
			{
				return this.Runtime.Module;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000021F9 File Offset: 0x000003F9
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002201 File Offset: 0x00000401
		public VMRuntime Runtime { get; set; }

		// Token: 0x0600003C RID: 60 RVA: 0x00005180 File Offset: 0x00003380
		public Virtualizer(int seed, bool debug)
		{
			this.Runtime = null;
			this.seed = seed;
			this.debug = debug;
			this.instantiation.ShouldInstantiate += (MethodSpec spec) => this.doInstantiation.Contains(spec.Method.ResolveMethodDefThrow());
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000051F0 File Offset: 0x000033F0
		public void Initialize()
		{
			ModuleDefMD moduleDefMD = ModuleDefMD.Load("KoiVM.Runtime.dll", null);
			moduleDefMD.Name = "";
			this.Runtime = new VMRuntime(this, moduleDefMD);
			this.vr = new MethodVirtualizer(this.Runtime);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000523C File Offset: 0x0000343C
		public void AddModule(ModuleDef module)
		{
			foreach (Tuple<MethodDef, bool> method in new Scanner(module).Scan())
			{
				this.AddMethod(method.Item1, method.Item2);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000052A0 File Offset: 0x000034A0
		public void AddMethod(MethodDef method, bool isExport)
		{
			bool flag = !method.HasBody;
			if (!flag)
			{
				bool hasGenericParameters = method.HasGenericParameters;
				if (hasGenericParameters)
				{
					bool flag2 = !isExport;
					if (flag2)
					{
						this.doInstantiation.Add(method);
					}
				}
				else
				{
					this.methodList.Add(method, isExport);
				}
				bool flag3 = !isExport;
				if (flag3)
				{
					TypeSig thisParam = (method.HasThis ? method.Parameters[0].Type : null);
					TypeDef declType = method.DeclaringType;
					declType.Methods.Remove(method);
					bool flag4 = method.SemanticsAttributes > MethodSemanticsAttributes.None;
					if (flag4)
					{
						foreach (PropertyDef prop in declType.Properties)
						{
							bool flag5 = prop.GetMethod == method;
							if (flag5)
							{
								prop.GetMethod = null;
							}
							bool flag6 = prop.SetMethod == method;
							if (flag6)
							{
								prop.SetMethod = null;
							}
						}
						foreach (EventDef evt in declType.Events)
						{
							bool flag7 = evt.AddMethod == method;
							if (flag7)
							{
								evt.AddMethod = null;
							}
							bool flag8 = evt.RemoveMethod == method;
							if (flag8)
							{
								evt.RemoveMethod = null;
							}
							bool flag9 = evt.InvokeMethod == method;
							if (flag9)
							{
								evt.InvokeMethod = null;
							}
						}
					}
					method.DeclaringType2 = declType;
					bool flag10 = thisParam != null;
					if (flag10)
					{
						method.Parameters[0].Type = thisParam;
					}
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005470 File Offset: 0x00003670
		public IEnumerable<MethodDef> GetMethods()
		{
			return this.methodList.Keys;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00005490 File Offset: 0x00003690
		public void ProcessMethods(ModuleDef module, Action<int, int> progress = null)
		{
			bool flag = this.processed.Contains(module);
			if (flag)
			{
				throw new InvalidOperationException("Module already processed.");
			}
			bool flag2 = progress == null;
			if (flag2)
			{
				progress = delegate(int num, int total)
				{
				};
			}
			List<MethodDef> targets = this.methodList.Keys.Where((MethodDef method) => method.Module == module).ToList<MethodDef>();
			Action<MethodSpec, MethodDef> <>9__2;
			for (int i = 0; i < targets.Count; i++)
			{
				MethodDef method2 = targets[i];
				GenericInstantiation genericInstantiation = this.instantiation;
				MethodDef methodDef = method2;
				Action<MethodSpec, MethodDef> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(MethodSpec spec, MethodDef instantation)
					{
						bool flag3 = instantation.Module == module || this.processed.Contains(instantation.Module);
						if (flag3)
						{
							targets.Add(instantation);
						}
						this.methodList[instantation] = false;
					});
				}
				genericInstantiation.EnsureInstantiation(methodDef, action);
				this.ProcessMethod(method2, this.methodList[method2]);
				progress(i, targets.Count);
			}
			progress(targets.Count, targets.Count);
			this.processed.Add(module);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000055D8 File Offset: 0x000037D8
		public IModuleWriterListener CommitModule(ModuleDefMD module, Action<int, int> progress = null)
		{
			bool flag = progress == null;
			if (flag)
			{
				progress = delegate(int num, int total)
				{
				};
			}
			MethodDef[] methods = this.methodList.Keys.Where((MethodDef method) => method.Module == module).ToArray<MethodDef>();
			for (int i = 0; i < methods.Length; i++)
			{
				MethodDef method2 = methods[i];
				this.PostProcessMethod(method2, this.methodList[method2]);
				progress(i, this.methodList.Count);
			}
			progress(methods.Length, methods.Length);
			return this.Runtime.CommitModule(module);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000220A File Offset: 0x0000040A
		public void CommitRuntime(ModuleDef targetModule = null)
		{
			this.Runtime.CommitRuntime(targetModule);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000221A File Offset: 0x0000041A
		private void ProcessMethod(MethodDef method, bool isExport)
		{
			this.vr.Run(method, isExport);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000056A8 File Offset: 0x000038A8
		private void PostProcessMethod(MethodDef method, bool isExport)
		{
			ScopeBlock scope = this.Runtime.LookupMethod(method);
			ILPostTransformer ilTransformer = new ILPostTransformer(method, scope, this.Runtime);
			ilTransformer.Transform();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000056D8 File Offset: 0x000038D8
		public string SaveRuntime(string directory)
		{
			string rtPath = Path.Combine(directory, this.runtimeName + ".dll");
			File.WriteAllBytes(rtPath, this.Runtime.RuntimeLibrary);
			bool flag = this.Runtime.RuntimeSymbols.Length != 0;
			if (flag)
			{
				File.WriteAllBytes(Path.ChangeExtension(rtPath, "pdb"), this.Runtime.RuntimeSymbols);
			}
			return rtPath;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00005744 File Offset: 0x00003944
		bool IVMSettings.IsExported(MethodDef method)
		{
			bool ret;
			bool flag = !this.methodList.TryGetValue(method, out ret);
			return !flag && ret;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00005770 File Offset: 0x00003970
		bool IVMSettings.IsVirtualized(MethodDef method)
		{
			return this.methodList.ContainsKey(method);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00005790 File Offset: 0x00003990
		int IVMSettings.Seed
		{
			get
			{
				return this.seed;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000057A8 File Offset: 0x000039A8
		bool IVMSettings.IsDebug
		{
			get
			{
				return this.debug;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000222B File Offset: 0x0000042B
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002233 File Offset: 0x00000433
		public bool ExportDbgInfo { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000223C File Offset: 0x0000043C
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002244 File Offset: 0x00000444
		public bool DoStackWalk { get; set; }

		// Token: 0x0400000F RID: 15
		private MethodVirtualizer vr;

		// Token: 0x04000010 RID: 16
		private string runtimeName;

		// Token: 0x04000011 RID: 17
		private Dictionary<MethodDef, bool> methodList = new Dictionary<MethodDef, bool>();

		// Token: 0x04000012 RID: 18
		private HashSet<ModuleDef> processed = new HashSet<ModuleDef>();

		// Token: 0x04000013 RID: 19
		private HashSet<MethodDef> doInstantiation = new HashSet<MethodDef>();

		// Token: 0x04000014 RID: 20
		private GenericInstantiation instantiation = new GenericInstantiation();

		// Token: 0x04000015 RID: 21
		private int seed;

		// Token: 0x04000016 RID: 22
		private bool debug;
	}
}
