using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using KoiVM.Runtime.Data;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x02000073 RID: 115
	internal static class VMTrampoline
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0000B988 File Offset: 0x00009B88
		static VMTrampoline()
		{
			foreach (MethodInfo method in typeof(VMEntry).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
			{
				bool flag = method.ReturnType != typeof(void) && method.GetParameters().Length > 4;
				if (flag)
				{
					VMTrampoline.entryStubNormal = method;
				}
				else
				{
					VMTrampoline.entryStubTyped = method;
				}
			}
			VMTrampoline.getDesc = (VMTrampoline.GetMethodDescriptor)Delegate.CreateDelegate(typeof(VMTrampoline.GetMethodDescriptor), typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000BA28 File Offset: 0x00009C28
		public static IntPtr CreateTrampoline(Module module, ulong codeAdr, uint key, VMFuncSig sig, uint sigId)
		{
			object dm = VMTrampoline.trampolines[codeAdr];
			bool flag = dm != null;
			IntPtr intPtr;
			if (flag)
			{
				intPtr = VMTrampoline.getDesc((DynamicMethod)dm).GetFunctionPointer();
			}
			else
			{
				Hashtable hashtable = VMTrampoline.trampolines;
				lock (hashtable)
				{
					dm = (DynamicMethod)VMTrampoline.trampolines[codeAdr];
					bool flag2 = dm != null;
					if (flag2)
					{
						intPtr = VMTrampoline.getDesc((DynamicMethod)dm).GetFunctionPointer();
					}
					else
					{
						bool flag3 = VMTrampoline.ShouldBeTyped(sig);
						if (flag3)
						{
							dm = VMTrampoline.CreateTrampolineTyped(VMInstance.GetModuleId(module), codeAdr, key, sig, sigId);
						}
						else
						{
							dm = VMTrampoline.CreateTrampolineNormal(VMInstance.GetModuleId(module), codeAdr, key, sig, sigId);
						}
						VMTrampoline.trampolines[codeAdr] = dm;
						intPtr = VMTrampoline.getDesc((DynamicMethod)dm).GetFunctionPointer();
					}
				}
			}
			return intPtr;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000BB30 File Offset: 0x00009D30
		private static bool ShouldBeTyped(VMFuncSig sig)
		{
			foreach (Type param in sig.ParamTypes)
			{
				bool isByRef = param.IsByRef;
				if (isByRef)
				{
					return true;
				}
			}
			return sig.RetType.IsByRef;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000BB7C File Offset: 0x00009D7C
		private static DynamicMethod CreateTrampolineNormal(int moduleId, ulong codeAdr, uint key, VMFuncSig sig, uint sigId)
		{
			DynamicMethod dm = new DynamicMethod("", sig.RetType, sig.ParamTypes, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldc_I4, moduleId);
			gen.Emit(OpCodes.Ldc_I8, (long)codeAdr);
			gen.Emit(OpCodes.Ldc_I4, (int)key);
			gen.Emit(OpCodes.Ldc_I4, (int)sigId);
			gen.Emit(OpCodes.Ldc_I4, sig.ParamTypes.Length);
			gen.Emit(OpCodes.Newarr, typeof(object));
			for (int i = 0; i < sig.ParamTypes.Length; i++)
			{
				gen.Emit(OpCodes.Dup);
				gen.Emit(OpCodes.Ldc_I4, i);
				gen.Emit(OpCodes.Ldarg, i);
				bool isValueType = sig.ParamTypes[i].IsValueType;
				if (isValueType)
				{
					gen.Emit(OpCodes.Box, sig.ParamTypes[i]);
				}
				gen.Emit(OpCodes.Stelem_Ref);
			}
			gen.Emit(OpCodes.Call, VMTrampoline.entryStubNormal);
			bool flag = sig.RetType == typeof(void);
			if (flag)
			{
				gen.Emit(OpCodes.Pop);
			}
			else
			{
				bool isValueType2 = sig.RetType.IsValueType;
				if (isValueType2)
				{
					gen.Emit(OpCodes.Unbox_Any, sig.RetType);
				}
				else
				{
					gen.Emit(OpCodes.Castclass, sig.RetType);
				}
			}
			gen.Emit(OpCodes.Ret);
			return dm;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000BD00 File Offset: 0x00009F00
		private unsafe static DynamicMethod CreateTrampolineTyped(int moduleId, ulong codeAdr, uint key, VMFuncSig sig, uint sigId)
		{
			DynamicMethod dm = new DynamicMethod("", sig.RetType, sig.ParamTypes, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldc_I4, moduleId);
			gen.Emit(OpCodes.Ldc_I8, (long)codeAdr);
			gen.Emit(OpCodes.Ldc_I4, (int)key);
			gen.Emit(OpCodes.Ldc_I4, (int)sigId);
			gen.Emit(OpCodes.Ldc_I4, sig.ParamTypes.Length);
			gen.Emit(OpCodes.Newarr, typeof(void*));
			for (int i = 0; i < sig.ParamTypes.Length; i++)
			{
				gen.Emit(OpCodes.Dup);
				gen.Emit(OpCodes.Ldc_I4, i);
				bool isByRef = sig.ParamTypes[i].IsByRef;
				if (isByRef)
				{
					gen.Emit(OpCodes.Ldarg, i);
					gen.Emit(OpCodes.Mkrefany, sig.ParamTypes[i].GetElementType());
				}
				else
				{
					gen.Emit(OpCodes.Ldarga, i);
					gen.Emit(OpCodes.Mkrefany, sig.ParamTypes[i]);
				}
				LocalBuilder local = gen.DeclareLocal(typeof(TypedReference));
				gen.Emit(OpCodes.Stloc, local);
				gen.Emit(OpCodes.Ldloca, local);
				gen.Emit(OpCodes.Conv_I);
				gen.Emit(OpCodes.Stelem_I);
			}
			bool flag = sig.RetType != typeof(void);
			if (flag)
			{
				LocalBuilder retVar = gen.DeclareLocal(sig.RetType);
				LocalBuilder retRef = gen.DeclareLocal(typeof(TypedReference));
				gen.Emit(OpCodes.Ldloca, retVar);
				gen.Emit(OpCodes.Mkrefany, sig.RetType);
				gen.Emit(OpCodes.Stloc, retRef);
				gen.Emit(OpCodes.Ldloca, retRef);
				gen.Emit(OpCodes.Call, VMTrampoline.entryStubTyped);
				gen.Emit(OpCodes.Ldloc, retVar);
			}
			else
			{
				gen.Emit(OpCodes.Ldnull);
				gen.Emit(OpCodes.Call, VMTrampoline.entryStubTyped);
			}
			gen.Emit(OpCodes.Ret);
			return dm;
		}

		// Token: 0x0400004D RID: 77
		private static readonly VMTrampoline.GetMethodDescriptor getDesc;

		// Token: 0x0400004E RID: 78
		private static readonly MethodInfo entryStubNormal;

		// Token: 0x0400004F RID: 79
		private static readonly MethodInfo entryStubTyped;

		// Token: 0x04000050 RID: 80
		private static readonly Hashtable trampolines = new Hashtable();

		// Token: 0x02000086 RID: 134
		// (Invoke) Token: 0x060001D2 RID: 466
		private delegate RuntimeMethodHandle GetMethodDescriptor(DynamicMethod dm);
	}
}
