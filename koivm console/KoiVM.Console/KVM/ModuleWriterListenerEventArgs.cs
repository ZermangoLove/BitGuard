using System;
using dnlib.DotNet.Writer;

namespace KVM
{
	// Token: 0x02000007 RID: 7
	public class ModuleWriterListenerEventArgs : EventArgs
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002089 File Offset: 0x00000289
		public ModuleWriterListenerEventArgs(ModuleWriterEvent evt)
		{
			this.WriterEvent = evt;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000209B File Offset: 0x0000029B
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000020A3 File Offset: 0x000002A3
		public ModuleWriterEvent WriterEvent { get; private set; }
	}
}
