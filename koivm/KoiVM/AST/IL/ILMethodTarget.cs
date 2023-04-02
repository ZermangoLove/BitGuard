using System;
using dnlib.DotNet;
using KoiVM.RT;

namespace KoiVM.AST.IL
{
	// Token: 0x02000140 RID: 320
	public class ILMethodTarget : IILOperand, IHasOffset
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x00003C08 File Offset: 0x00001E08
		public ILMethodTarget(MethodDef target)
		{
			this.Target = target;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00003C1A File Offset: 0x00001E1A
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x00003C22 File Offset: 0x00001E22
		public MethodDef Target { get; set; }

		// Token: 0x06000576 RID: 1398 RVA: 0x00003C2B File Offset: 0x00001E2B
		public void Resolve(VMRuntime runtime)
		{
			runtime.LookupMethod(this.Target, out this.methodEntry);
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001CEF8 File Offset: 0x0001B0F8
		public uint Offset
		{
			get
			{
				return (this.methodEntry == null) ? 0U : this.methodEntry.Content[0].Offset;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001CF2C File Offset: 0x0001B12C
		public override string ToString()
		{
			return this.Target.ToString();
		}

		// Token: 0x04000257 RID: 599
		private ILBlock methodEntry;
	}
}
