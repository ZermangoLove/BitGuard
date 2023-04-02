using System;
using System.Collections.Generic;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace KVM
{
	// Token: 0x02000005 RID: 5
	public class KVMTask
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000027D8 File Offset: 0x000009D8
		public void Exceute(ModuleDef module, string outPath, string snPath, string snPass)
		{
			this.assemblyReferencesAdder(module);
			Utils.ModuleWriterListener = new ModuleWriterListener();
			Utils.ModuleWriterOptions = new ModuleWriterOptions(module);
			Utils.ModuleWriterOptions.Listener = Utils.ModuleWriterListener;
			Utils.ModuleWriterOptions.Logger = DummyLogger.NoThrowInstance;
			bool flag = File.Exists(snPath);
			if (flag)
			{
				StrongNameKey strongNameKey = Utils.LoadSNKey(snPath, snPass);
				Utils.ModuleWriterOptions.InitializeStrongNameSigning(module, strongNameKey);
			}
			new InitializePhase().InitializeP(module);
			MemoryStream memoryStream = new MemoryStream();
			module.Write(memoryStream, Utils.ModuleWriterOptions);
			File.WriteAllBytes(outPath, memoryStream.ToArray());
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002874 File Offset: 0x00000A74
		private void assemblyReferencesAdder(ModuleDef moduleDefMD)
		{
			AssemblyResolver assemblyResolver = new AssemblyResolver
			{
				EnableTypeDefCache = true
			};
			ModuleContext moduleContext = new ModuleContext(assemblyResolver);
			assemblyResolver.DefaultModuleContext = moduleContext;
			IEnumerable<AssemblyRef> assemblyRefs = moduleDefMD.GetAssemblyRefs();
			moduleDefMD.Context = moduleContext;
			foreach (AssemblyRef assemblyRef in assemblyRefs)
			{
				try
				{
					bool flag = assemblyRef == null;
					if (!flag)
					{
						AssemblyDef assemblyDef = assemblyResolver.Resolve(assemblyRef.FullName, moduleDefMD);
						bool flag2 = assemblyDef == null;
						if (!flag2)
						{
							moduleDefMD.Context.AssemblyResolver.AddToCache(assemblyDef);
						}
					}
				}
				catch
				{
				}
			}
		}
	}
}
