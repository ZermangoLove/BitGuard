using System;
using System.Reflection;
using System.Reflection.Emit;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006A RID: 106
	internal class EHHelper
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00009FBC File Offset: 0x000081BC
		private static bool BuildExceptionDispatchInfo(Type type)
		{
			try
			{
				MethodInfo capture = type.GetMethod("Capture");
				MethodInfo thr = type.GetMethod("Throw");
				DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
				{
					typeof(Exception),
					typeof(string),
					typeof(bool)
				});
				ILGenerator ilGen = dm.GetILGenerator();
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Call, capture);
				ilGen.Emit(OpCodes.Call, thr);
				ilGen.Emit(OpCodes.Ret);
				EHHelper.rethrow = (EHHelper.Throw)dm.CreateDelegate(typeof(EHHelper.Throw));
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A09C File Offset: 0x0000829C
		private static bool BuildInternalPreserve(Type type)
		{
			try
			{
				string at = (string)typeof(Environment).InvokeMember("GetResourceString", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[] { "Word_At" });
				MethodInfo preserve = type.GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
				FieldInfo field = type.GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
				MethodInfo stackTrace = type.GetProperty("StackTrace", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
				MethodInfo fmt = typeof(string).GetMethod("Format", new Type[]
				{
					typeof(string),
					typeof(object),
					typeof(object)
				});
				DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
				{
					typeof(Exception),
					typeof(string),
					typeof(bool)
				}, true);
				ILGenerator ilGen = dm.GetILGenerator();
				Label lbl = ilGen.DefineLabel();
				Label lbl2 = ilGen.DefineLabel();
				Label lbl3 = ilGen.DefineLabel();
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Ldfld, field);
				ilGen.Emit(OpCodes.Brtrue, lbl2);
				ilGen.Emit(OpCodes.Callvirt, stackTrace);
				ilGen.Emit(OpCodes.Br, lbl3);
				ilGen.MarkLabel(lbl2);
				ilGen.Emit(OpCodes.Ldfld, field);
				ilGen.MarkLabel(lbl3);
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Call, preserve);
				ilGen.Emit(OpCodes.Stfld, field);
				ilGen.Emit(OpCodes.Ldarg_1);
				ilGen.Emit(OpCodes.Brfalse, lbl);
				ilGen.Emit(OpCodes.Ldarg_2);
				ilGen.Emit(OpCodes.Brtrue, lbl);
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Ldstr, string.Concat(new string[]
				{
					"{1}",
					Environment.NewLine,
					"   ",
					at,
					" KoiVM [{0}]",
					Environment.NewLine
				}));
				ilGen.Emit(OpCodes.Ldarg_1);
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Ldfld, field);
				ilGen.Emit(OpCodes.Call, fmt);
				ilGen.Emit(OpCodes.Stfld, field);
				ilGen.Emit(OpCodes.Throw);
				ilGen.MarkLabel(lbl);
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Throw);
				EHHelper.rethrow = (EHHelper.Throw)dm.CreateDelegate(typeof(EHHelper.Throw));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
			return true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A3C0 File Offset: 0x000085C0
		static EHHelper()
		{
			bool flag = EHHelper.BuildInternalPreserve(typeof(Exception));
			if (!flag)
			{
				Type type = Type.GetType("System.Runtime.ExceptionServices.ExceptionDispatchInfo");
				bool flag2 = type != null && EHHelper.BuildExceptionDispatchInfo(type);
				if (!flag2)
				{
					EHHelper.rethrow = null;
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A410 File Offset: 0x00008610
		public static void Rethrow(Exception ex, string tokens)
		{
			bool flag = tokens == null;
			if (flag)
			{
				throw ex;
			}
			bool r = ex.Data.Contains(EHHelper.RethrowKey);
			bool flag2 = !r;
			if (flag2)
			{
				ex.Data[EHHelper.RethrowKey] = EHHelper.RethrowKey;
			}
			bool flag3 = EHHelper.rethrow != null;
			if (flag3)
			{
				EHHelper.rethrow(ex, tokens, r);
			}
			throw ex;
		}

		// Token: 0x0400003B RID: 59
		private static EHHelper.Throw rethrow;

		// Token: 0x0400003C RID: 60
		private static readonly object RethrowKey = new object();

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x060001B2 RID: 434
		private delegate void Throw(Exception ex, string ip, bool rethrow);
	}
}
