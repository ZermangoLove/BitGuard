using System;
using System.Diagnostics;
using dnlib.DotNet.Writer;

namespace KVM
{
	// Token: 0x02000006 RID: 6
	public class ModuleWriterListener : IModuleWriterListener
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000293C File Offset: 0x00000B3C
		void IModuleWriterListener.OnWriterEvent(ModuleWriterBase writer, ModuleWriterEvent evt)
		{
			bool flag = this.OnWriterEvent != null;
			bool flag2 = flag;
			if (flag2)
			{
				this.OnWriterEvent(writer, new ModuleWriterListenerEventArgs(evt));
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000F RID: 15 RVA: 0x00002970 File Offset: 0x00000B70
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x000029A8 File Offset: 0x00000BA8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ModuleWriterListenerEventArgs> OnWriterEvent;
	}
}
