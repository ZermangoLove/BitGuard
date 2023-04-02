using System;
using dnlib.DotNet;

namespace KoiVM
{
	// Token: 0x02000004 RID: 4
	public interface IVMSettings
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16
		int Seed { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17
		bool IsDebug { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18
		bool ExportDbgInfo { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19
		bool DoStackWalk { get; }

		// Token: 0x06000014 RID: 20
		bool IsVirtualized(MethodDef method);

		// Token: 0x06000015 RID: 21
		bool IsExported(MethodDef method);
	}
}
