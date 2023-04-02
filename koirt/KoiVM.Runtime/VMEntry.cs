using System;
using System.Reflection;

namespace KoiVM.Runtime
{
	// Token: 0x02000004 RID: 4
	public class VMEntry
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000225C File Offset: 0x0000045C
		public static object Run(uint id, object[] args)
		{
			Module module = MethodBase.GetCurrentMethod().Module;
			return VMInstance.Instance(module).Run(id, args);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002288 File Offset: 0x00000488
		public unsafe static void Run(uint id, void*[] typedRefs, void* retTypedRef)
		{
			Module module = MethodBase.GetCurrentMethod().Module;
			VMInstance.Instance(module).Run(id, typedRefs, retTypedRef);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022B0 File Offset: 0x000004B0
		internal static object RunInternal(int moduleId, ulong codeAddr, uint key, uint sigId, object[] args)
		{
			return VMInstance.Instance(moduleId).Run(codeAddr, key, sigId, args);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022D2 File Offset: 0x000004D2
		internal unsafe static void RunInternal(int moduleId, ulong codeAddr, uint key, uint sigId, void*[] typedRefs, void* retTypedRef)
		{
			VMInstance.Instance(moduleId).Run(codeAddr, key, sigId, typedRefs, retTypedRef);
		}
	}
}
