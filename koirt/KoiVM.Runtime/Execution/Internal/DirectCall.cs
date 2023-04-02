using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x02000070 RID: 112
	internal static class DirectCall
	{
		// Token: 0x06000181 RID: 385 RVA: 0x0000A7E4 File Offset: 0x000089E4
		static DirectCall()
		{
			foreach (MethodInfo method in typeof(IReference).GetMethods())
			{
				foreach (ParameterInfo param in method.GetParameters())
				{
					bool flag = param.ParameterType == typeof(TypedRefPtr);
					if (flag)
					{
						DirectCall.refToTypedRef = method;
						break;
					}
				}
				bool flag2 = DirectCall.refToTypedRef != null;
				if (flag2)
				{
					break;
				}
			}
			foreach (MethodInfo method2 in typeof(TypedReferenceHelpers).GetMethods())
			{
				bool flag3 = method2.GetParameters()[0].ParameterType == typeof(TypedRefPtr);
				if (flag3)
				{
					DirectCall.castTypedRef = method2;
					break;
				}
			}
			foreach (ConstructorInfo ctor in typeof(TypedRef).GetConstructors())
			{
				foreach (ParameterInfo param2 in ctor.GetParameters())
				{
					bool flag4 = param2.ParameterType == typeof(TypedReference);
					if (flag4)
					{
						DirectCall.newTypedRef = ctor;
						break;
					}
				}
				bool flag5 = DirectCall.newTypedRef != null;
				if (flag5)
				{
					break;
				}
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A964 File Offset: 0x00008B64
		public static MethodBase GetDirectInvocationProxy(MethodBase method)
		{
			MethodBase proxy = (MethodBase)DirectCall.directProxies[method];
			bool flag = proxy != null;
			MethodBase methodBase;
			if (flag)
			{
				methodBase = proxy;
			}
			else
			{
				Hashtable hashtable = DirectCall.directProxies;
				lock (hashtable)
				{
					proxy = (MethodBase)DirectCall.directProxies[method];
					bool flag2 = proxy != null;
					if (flag2)
					{
						methodBase = proxy;
					}
					else
					{
						ParameterInfo[] parameters = method.GetParameters();
						Type[] paramTypes = new Type[parameters.Length + (method.IsStatic ? 0 : 1)];
						for (int i = 0; i < paramTypes.Length; i++)
						{
							bool isStatic = method.IsStatic;
							if (isStatic)
							{
								paramTypes[i] = parameters[i].ParameterType;
							}
							else
							{
								bool flag3 = i == 0;
								if (flag3)
								{
									paramTypes[0] = method.DeclaringType;
								}
								else
								{
									paramTypes[i] = parameters[i - 1].ParameterType;
								}
							}
						}
						Type retType = ((method is MethodInfo) ? ((MethodInfo)method).ReturnType : typeof(void));
						DynamicMethod dm = new DynamicMethod("", retType, paramTypes, Unverifier.Module, true);
						ILGenerator gen = dm.GetILGenerator();
						for (int j = 0; j < paramTypes.Length; j++)
						{
							bool flag4 = !method.IsStatic && j == 0 && paramTypes[0].IsValueType;
							if (flag4)
							{
								gen.Emit(OpCodes.Ldarga, j);
							}
							else
							{
								gen.Emit(OpCodes.Ldarg, j);
							}
						}
						bool flag5 = method is MethodInfo;
						if (flag5)
						{
							gen.Emit(OpCodes.Call, (MethodInfo)method);
						}
						else
						{
							gen.Emit(OpCodes.Call, (ConstructorInfo)method);
						}
						gen.Emit(OpCodes.Ret);
						DirectCall.directProxies[method] = dm;
						methodBase = dm;
					}
				}
			}
			return methodBase;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000AB5C File Offset: 0x00008D5C
		public static DirectCall.TypedInvocation GetTypedInvocationProxy(MethodBase method, OpCode opCode, Type constrainType)
		{
			bool flag = constrainType == null;
			object key;
			Hashtable table;
			if (flag)
			{
				key = new KeyValuePair<MethodBase, OpCode>(method, opCode);
				table = DirectCall.typedProxies;
			}
			else
			{
				key = new KeyValuePair<MethodBase, Type>(method, constrainType);
				table = DirectCall.constrainedProxies;
			}
			DirectCall.TypedInvocation proxy = (DirectCall.TypedInvocation)table[key];
			bool flag2 = proxy != null;
			DirectCall.TypedInvocation typedInvocation;
			if (flag2)
			{
				typedInvocation = proxy;
			}
			else
			{
				Hashtable hashtable = DirectCall.typedProxies;
				lock (hashtable)
				{
					proxy = (DirectCall.TypedInvocation)table[key];
					bool flag3 = proxy != null;
					if (flag3)
					{
						typedInvocation = proxy;
					}
					else
					{
						ParameterInfo[] parameters = method.GetParameters();
						bool flag4 = opCode != OpCodes.Newobj;
						Type[] paramTypes;
						if (flag4)
						{
							paramTypes = new Type[parameters.Length + (method.IsStatic ? 0 : 1) + 1];
							for (int i = 0; i < paramTypes.Length - 1; i++)
							{
								bool isStatic = method.IsStatic;
								if (isStatic)
								{
									paramTypes[i] = parameters[i].ParameterType;
								}
								else
								{
									bool flag5 = i == 0;
									if (flag5)
									{
										bool flag6 = constrainType != null;
										if (flag6)
										{
											paramTypes[0] = constrainType.MakeByRefType();
										}
										else
										{
											bool isValueType = method.DeclaringType.IsValueType;
											if (isValueType)
											{
												paramTypes[0] = method.DeclaringType.MakeByRefType();
											}
											else
											{
												paramTypes[0] = method.DeclaringType;
											}
										}
									}
									else
									{
										paramTypes[i] = parameters[i - 1].ParameterType;
									}
								}
							}
						}
						else
						{
							paramTypes = new Type[parameters.Length + 1];
							for (int j = 0; j < paramTypes.Length - 1; j++)
							{
								paramTypes[j] = parameters[j].ParameterType;
							}
						}
						Type retType = ((method is MethodInfo) ? ((MethodInfo)method).ReturnType : typeof(void));
						bool flag7 = opCode == OpCodes.Newobj;
						if (flag7)
						{
							retType = method.DeclaringType;
						}
						DynamicMethod dm = new DynamicMethod("", typeof(object), new Type[]
						{
							typeof(VMContext),
							typeof(IReference[]),
							typeof(Type[])
						}, Unverifier.Module, true);
						ILGenerator gen = dm.GetILGenerator();
						for (int k = 0; k < paramTypes.Length - 1; k++)
						{
							Type paramType = paramTypes[k];
							bool isByRef = paramType.IsByRef;
							bool flag8 = isByRef;
							if (flag8)
							{
								paramType = paramType.GetElementType();
							}
							LocalBuilder typedRefLocal = gen.DeclareLocal(typeof(TypedReference));
							gen.Emit(OpCodes.Ldarg_1);
							gen.Emit(OpCodes.Ldc_I4, k);
							gen.Emit(OpCodes.Ldelem_Ref);
							gen.Emit(OpCodes.Ldarg_0);
							gen.Emit(OpCodes.Ldloca, typedRefLocal);
							gen.Emit(OpCodes.Ldarg_2);
							gen.Emit(OpCodes.Ldc_I4, k);
							gen.Emit(OpCodes.Ldelem_Ref);
							gen.Emit(OpCodes.Callvirt, DirectCall.refToTypedRef);
							gen.Emit(OpCodes.Ldloca, typedRefLocal);
							gen.Emit(OpCodes.Ldarg_2);
							gen.Emit(OpCodes.Ldc_I4, k);
							gen.Emit(OpCodes.Ldelem_Ref);
							gen.Emit(OpCodes.Call, DirectCall.castTypedRef);
							gen.Emit(OpCodes.Ldloc, typedRefLocal);
							gen.Emit(OpCodes.Refanyval, paramType);
							bool flag9 = !isByRef;
							if (flag9)
							{
								gen.Emit(OpCodes.Ldobj, paramType);
							}
						}
						bool flag10 = constrainType != null;
						if (flag10)
						{
							gen.Emit(OpCodes.Constrained, constrainType);
						}
						bool flag11 = method is MethodInfo;
						if (flag11)
						{
							gen.Emit(opCode, (MethodInfo)method);
						}
						else
						{
							gen.Emit(opCode, (ConstructorInfo)method);
						}
						bool isByRef2 = retType.IsByRef;
						if (isByRef2)
						{
							gen.Emit(OpCodes.Mkrefany, retType.GetElementType());
							gen.Emit(OpCodes.Newobj, DirectCall.newTypedRef);
						}
						else
						{
							bool flag12 = retType == typeof(void);
							if (flag12)
							{
								gen.Emit(OpCodes.Ldnull);
							}
							else
							{
								bool isValueType2 = retType.IsValueType;
								if (isValueType2)
								{
									gen.Emit(OpCodes.Box, retType);
								}
							}
						}
						gen.Emit(OpCodes.Ret);
						proxy = (DirectCall.TypedInvocation)dm.CreateDelegate(typeof(DirectCall.TypedInvocation));
						table[key] = proxy;
						typedInvocation = proxy;
					}
				}
			}
			return typedInvocation;
		}

		// Token: 0x04000040 RID: 64
		private static Hashtable directProxies = new Hashtable();

		// Token: 0x04000041 RID: 65
		private static Hashtable typedProxies = new Hashtable();

		// Token: 0x04000042 RID: 66
		private static Hashtable constrainedProxies = new Hashtable();

		// Token: 0x04000043 RID: 67
		private static MethodInfo refToTypedRef;

		// Token: 0x04000044 RID: 68
		private static MethodInfo castTypedRef;

		// Token: 0x04000045 RID: 69
		private static ConstructorInfo newTypedRef;

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x060001BA RID: 442
		public delegate object TypedInvocation(VMContext ctx, IReference[] refs, Type[] types);
	}
}
