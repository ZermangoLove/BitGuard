using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x02000072 RID: 114
	internal static class Unverifier
	{
		// Token: 0x06000190 RID: 400 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		static Unverifier()
		{
			AssemblyBuilder asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Fish"), AssemblyBuilderAccess.Run);
			ModuleBuilder mod = asm.DefineDynamicModule("Fish");
			CustomAttributeBuilder att = new CustomAttributeBuilder(typeof(SecurityPermissionAttribute).GetConstructor(new Type[] { typeof(SecurityAction) }), new object[] { SecurityAction.Assert }, new PropertyInfo[] { typeof(SecurityPermissionAttribute).GetProperty("SkipVerification") }, new object[] { true });
			mod.SetCustomAttribute(att);
			Unverifier.Module = mod.DefineType(" ").CreateType().Module;
		}

		// Token: 0x0400004C RID: 76
		public static readonly Module Module;
	}
}
