using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM
{
	// Token: 0x02000006 RID: 6
	public class Scanner
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000021A0 File Offset: 0x000003A0
		public Scanner(ModuleDef module)
			: this(module, null)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000021AC File Offset: 0x000003AC
		public Scanner(ModuleDef module, HashSet<MethodDef> methods)
		{
			this.module = module;
			this.methods = methods;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004A24 File Offset: 0x00002C24
		public IEnumerable<Tuple<MethodDef, bool>> Scan()
		{
			this.ScanMethods(new Action<MethodDef>(this.FindExclusion));
			this.ScanMethods(new Action<MethodDef>(this.ScanExport));
			this.ScanMethods(new Action<MethodDef>(this.PopulateResult));
			return this.results;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004A78 File Offset: 0x00002C78
		private void ScanMethods(Action<MethodDef> scanFunc)
		{
			foreach (TypeDef type in this.module.GetTypes())
			{
				foreach (MethodDef method in type.Methods)
				{
					scanFunc(method);
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004B08 File Offset: 0x00002D08
		private void FindExclusion(MethodDef method)
		{
			bool flag = !method.HasBody || (this.methods != null && !this.methods.Contains(method));
			if (flag)
			{
				this.exclude.Add(method);
			}
			else
			{
				bool hasGenericParameters = method.HasGenericParameters;
				if (hasGenericParameters)
				{
					foreach (Instruction instr in method.Body.Instructions)
					{
						IMethod target = instr.Operand as IMethod;
						bool flag2 = target != null && target.IsMethod && (target = target.ResolveMethodDef()) != null && (this.methods == null || this.methods.Contains((MethodDef)target));
						if (flag2)
						{
							this.export.Add((MethodDef)target);
						}
					}
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004C04 File Offset: 0x00002E04
		private void ScanExport(MethodDef method)
		{
			bool flag = !method.HasBody;
			if (!flag)
			{
				bool shouldExport = false;
				shouldExport |= method.IsPublic;
				shouldExport |= method.SemanticsAttributes > MethodSemanticsAttributes.None;
				shouldExport |= method.IsConstructor;
				shouldExport |= method.IsVirtual;
				shouldExport |= method.Module.EntryPoint == method;
				bool flag2 = shouldExport;
				if (flag2)
				{
					this.export.Add(method);
				}
				bool excluded = this.exclude.Contains(method) || method.DeclaringType.HasGenericParameters;
				foreach (Instruction instr in method.Body.Instructions)
				{
					bool flag3 = instr.OpCode == OpCodes.Callvirt || (instr.Operand is IMethod && excluded);
					if (flag3)
					{
						MethodDef target = ((IMethod)instr.Operand).ResolveMethodDef();
						bool flag4 = target != null && (this.methods == null || this.methods.Contains(target));
						if (flag4)
						{
							this.export.Add(target);
						}
					}
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004D48 File Offset: 0x00002F48
		private void PopulateResult(MethodDef method)
		{
			bool flag = this.exclude.Contains(method) || method.DeclaringType.HasGenericParameters;
			if (!flag)
			{
				this.results.Add(Tuple.Create<MethodDef, bool>(method, this.export.Contains(method)));
			}
		}

		// Token: 0x0400000A RID: 10
		private ModuleDef module;

		// Token: 0x0400000B RID: 11
		private HashSet<MethodDef> methods;

		// Token: 0x0400000C RID: 12
		private HashSet<MethodDef> exclude = new HashSet<MethodDef>();

		// Token: 0x0400000D RID: 13
		private HashSet<MethodDef> export = new HashSet<MethodDef>();

		// Token: 0x0400000E RID: 14
		private List<Tuple<MethodDef, bool>> results = new List<Tuple<MethodDef, bool>>();
	}
}
