using System;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000FF RID: 255
	[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
	internal static class RuntimePatcher
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00003225 File Offset: 0x00001425
		public static void Patch(ModuleDef runtime, bool debug, bool stackwalk)
		{
			RuntimePatcher.PatchDispatcher(runtime, debug, stackwalk);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001744C File Offset: 0x0001564C
		private static void PatchDispatcher(ModuleDef runtime, bool debug, bool stackwalk)
		{
			TypeDef dispatcher = runtime.Find(RTMap.VMDispatcher, true);
			MethodDef dispatcherRun = dispatcher.FindMethod(RTMap.VMRun);
			foreach (ExceptionHandler eh in dispatcherRun.Body.ExceptionHandlers)
			{
				bool flag = eh.HandlerType == ExceptionHandlerType.Catch;
				if (flag)
				{
					eh.CatchType = runtime.CorLibTypes.Object.ToTypeDefOrRef();
				}
			}
			RuntimePatcher.PatchDoThrow(dispatcher.FindMethod(RTMap.VMDispatcherDothrow).Body, debug, stackwalk);
			dispatcher.Methods.Remove(dispatcher.FindMethod(RTMap.VMDispatcherThrow));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00017518 File Offset: 0x00015718
		private static void PatchDoThrow(CilBody body, bool debug, bool stackwalk)
		{
			for (int i = 0; i < body.Instructions.Count; i++)
			{
				IMethod method = body.Instructions[i].Operand as IMethod;
				bool flag = method != null && method.Name == RTMap.VMDispatcherThrow;
				if (flag)
				{
					body.Instructions.RemoveAt(i);
				}
				else
				{
					bool flag2 = method != null && method.Name == RTMap.VMDispatcherGetIP;
					if (flag2)
					{
						bool flag3 = !debug;
						if (flag3)
						{
							body.Instructions.RemoveAt(i);
							body.Instructions[i - 1].OpCode = OpCodes.Ldnull;
							MethodDef def = method.ResolveMethodDefThrow();
							def.DeclaringType.Methods.Remove(def);
						}
						else if (stackwalk)
						{
							MethodDef def2 = method.ResolveMethodDefThrow();
							body.Instructions[i].Operand = def2.DeclaringType.FindMethod(RTMap.VMDispatcherStackwalk);
							def2.DeclaringType.Methods.Remove(def2);
						}
						else
						{
							MethodDef def3 = method.ResolveMethodDefThrow();
							def3 = def3.DeclaringType.FindMethod(RTMap.VMDispatcherStackwalk);
							def3.DeclaringType.Methods.Remove(def3);
						}
					}
				}
			}
		}
	}
}
