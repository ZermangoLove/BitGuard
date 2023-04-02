using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using KoiVM;

namespace KVM
{
	// Token: 0x02000003 RID: 3
	public class InitializePhase
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000228C File Offset: 0x0000048C
		public void InitializeP(ModuleDef module)
		{
			foreach (TypeDef typeDef in module.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					InitializePhase.methods.Add(methodDef);
				}
			}
			int num = new Random().Next(1, int.MaxValue);
			string text = "KoiVM.Runtime";
			ModuleDefMD moduleDefMD = ModuleDefMD.Load("KoiVM.Runtime.dll", null);
			moduleDefMD.Assembly.Name = text;
			moduleDefMD.Name = text + ".dll";
			Virtualizer virtualizer = new Virtualizer(num, false);
			virtualizer.ExportDbgInfo = false;
			virtualizer.DoStackWalk = false;
			virtualizer.Initialize();
			InitializePhase.VirtualizerKey = virtualizer;
			InitializePhase.MergeKey = module;
			virtualizer.CommitRuntime(module);
			ConstructorInfo constructor = typeof(InternalsVisibleToAttribute).GetConstructor(new Type[] { typeof(string) });
			bool flag = InitializePhase.methods.Count > 0;
			if (flag)
			{
				CustomAttribute customAttribute = new CustomAttribute((ICustomAttributeType)module.Import(constructor));
				customAttribute.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.String, virtualizer.RuntimeModule.Assembly.Name.String));
				module.Assembly.CustomAttributes.Add(customAttribute);
			}
			this.MarkPhase(module);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000244C File Offset: 0x0000064C
		public void MarkPhase(ModuleDef module)
		{
			string text = "nash";
			Virtualizer virtualizer = (Virtualizer)InitializePhase.VirtualizerKey;
			Dictionary<IMemberRef, IMemberRef> dictionary = new Dictionary<IMemberRef, IMemberRef>();
			TypeDef globalType = module.GlobalType;
			TypeDefUser typeDefUser = new TypeDefUser(globalType.Name);
			globalType.Name = text;
			globalType.BaseType = module.CorLibTypes.GetTypeRef("System", "Object");
			module.Types.Insert(0, typeDefUser);
			MethodDef methodDef = globalType.FindOrCreateStaticConstructor();
			MethodDef methodDef2 = typeDefUser.FindOrCreateStaticConstructor();
			methodDef.Name = text;
			methodDef.IsRuntimeSpecialName = false;
			methodDef.IsSpecialName = false;
			methodDef.Access = dnlib.DotNet.MethodAttributes.PrivateScope;
			methodDef2.Body = new CilBody(true, new List<Instruction>
			{
				Instruction.Create(OpCodes.Call, methodDef),
				Instruction.Create(OpCodes.Ret)
			}, new List<ExceptionHandler>(), new List<Local>());
			for (int i = 0; i < globalType.Methods.Count; i++)
			{
				MethodDef methodDef3 = globalType.Methods[i];
				bool isNative = methodDef3.IsNative;
				if (isNative)
				{
					MethodDefUser methodDefUser = new MethodDefUser(methodDef3.Name, methodDef3.MethodSig.Clone());
					methodDefUser.Attributes = dnlib.DotNet.MethodAttributes.Private | dnlib.DotNet.MethodAttributes.FamANDAssem | dnlib.DotNet.MethodAttributes.Static;
					methodDefUser.Body = new CilBody();
					methodDefUser.Body.Instructions.Add(new Instruction(OpCodes.Jmp, methodDef3));
					methodDefUser.Body.Instructions.Add(new Instruction(OpCodes.Ret));
					globalType.Methods[i] = methodDefUser;
					typeDefUser.Methods.Add(methodDef3);
					dictionary[methodDef3] = methodDefUser;
				}
			}
			InitializePhase.methods.Add(methodDef);
			Dictionary<ModuleDef, List<MethodDef>> dictionary2 = new Dictionary<ModuleDef, List<MethodDef>>();
			foreach (Tuple<MethodDef, bool> tuple in new Scanner(module, InitializePhase.methods).Scan())
			{
				virtualizer.AddMethod(tuple.Item1, tuple.Item2);
				dictionary2.AddListEntry(tuple.Item1.Module, tuple.Item1);
			}
			Utils.ModuleWriterListener.OnWriterEvent += new InitializePhase.Listener
			{
				vr = virtualizer,
				methods = dictionary2,
				refRepl = dictionary,
				module = module
			}.OnWriterEvent;
		}

		// Token: 0x04000001 RID: 1
		public static object VirtualizerKey = new object();

		// Token: 0x04000002 RID: 2
		public static object MergeKey = new object();

		// Token: 0x04000003 RID: 3
		public static HashSet<MethodDef> methods = new HashSet<MethodDef>();

		// Token: 0x02000004 RID: 4
		private class Listener
		{
			// Token: 0x06000009 RID: 9 RVA: 0x000026E0 File Offset: 0x000008E0
			public void OnWriterEvent(object sender, ModuleWriterListenerEventArgs e)
			{
				ModuleWriter moduleWriter = (ModuleWriter)sender;
				bool flag = this.commitListener != null;
				if (flag)
				{
					this.commitListener.OnWriterEvent(moduleWriter, e.WriterEvent);
				}
				bool flag2 = e.WriterEvent == ModuleWriterEvent.MDBeginWriteMethodBodies && this.methods.ContainsKey(moduleWriter.Module);
				if (flag2)
				{
					this.vr.ProcessMethods(moduleWriter.Module, null);
					foreach (KeyValuePair<IMemberRef, IMemberRef> keyValuePair in this.refRepl)
					{
						this.vr.Runtime.Descriptor.Data.ReplaceReference(keyValuePair.Key, keyValuePair.Value);
					}
					this.commitListener = this.vr.CommitModule((ModuleDefMD)this.module, null);
				}
			}

			// Token: 0x04000004 RID: 4
			public Virtualizer vr;

			// Token: 0x04000005 RID: 5
			public Dictionary<ModuleDef, List<MethodDef>> methods;

			// Token: 0x04000006 RID: 6
			public Dictionary<IMemberRef, IMemberRef> refRepl;

			// Token: 0x04000007 RID: 7
			private IModuleWriterListener commitListener = null;

			// Token: 0x04000008 RID: 8
			public ModuleDef module;
		}
	}
}
