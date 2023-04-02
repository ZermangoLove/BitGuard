using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using KoiVM.Runtime.Data;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000015 RID: 21
	internal class Ecall : IVCall
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003B78 File Offset: 0x00001D78
		public byte Code
		{
			get
			{
				return Constants.VCALL_ECALL;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003B90 File Offset: 0x00001D90
		private static object PopObject(VMContext ctx, Type type, ref uint sp)
		{
			VMStack stack = ctx.Stack;
			uint num = sp;
			sp = num - 1U;
			VMSlot arg = stack[num];
			bool flag = Type.GetTypeCode(type) == TypeCode.String && arg.O == null;
			object obj;
			if (flag)
			{
				obj = ctx.Instance.Data.LookupString(arg.U4);
			}
			else
			{
				obj = arg.ToObject(type);
			}
			return obj;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003BF4 File Offset: 0x00001DF4
		private unsafe static IReference PopRef(VMContext ctx, Type type, ref uint sp)
		{
			VMSlot arg = ctx.Stack[sp];
			bool isByRef = type.IsByRef;
			IReference reference;
			if (isByRef)
			{
				sp -= 1U;
				type = type.GetElementType();
				bool flag = arg.O is Pointer;
				if (flag)
				{
					void* ptr = Pointer.Unbox(arg.O);
					reference = new PointerRef(ptr);
				}
				else
				{
					bool flag2 = arg.O is IReference;
					if (flag2)
					{
						reference = (IReference)arg.O;
					}
					else
					{
						reference = new PointerRef(arg.U8);
					}
				}
			}
			else
			{
				bool flag3 = Type.GetTypeCode(type) == TypeCode.String && arg.O == null;
				if (flag3)
				{
					arg.O = ctx.Instance.Data.LookupString(arg.U4);
					ctx.Stack[sp] = arg;
				}
				uint num = sp;
				sp = num - 1U;
				reference = new StackRef(num);
			}
			return reference;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003CF4 File Offset: 0x00001EF4
		private static bool NeedTypedInvoke(VMContext ctx, uint sp, MethodBase method, bool isNewObj)
		{
			bool flag = !isNewObj && !method.IsStatic;
			if (flag)
			{
				bool isValueType = method.DeclaringType.IsValueType;
				if (isValueType)
				{
					return true;
				}
			}
			foreach (ParameterInfo param in method.GetParameters())
			{
				bool isByRef = param.ParameterType.IsByRef;
				if (isByRef)
				{
					return true;
				}
			}
			return method is MethodInfo && ((MethodInfo)method).ReturnType.IsByRef;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003D8C File Offset: 0x00001F8C
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot mSlot = ctx.Stack[sp--];
			uint mId = mSlot.U4 & 1073741823U;
			byte opCode = (byte)(mSlot.U4 >> 30);
			MethodBase targetMethod = (MethodBase)ctx.Instance.Data.LookupReference(mId);
			bool typedInvoke = opCode == Constants.ECALL_CALLVIRT_CONSTRAINED;
			bool flag = !typedInvoke;
			if (flag)
			{
				typedInvoke = Ecall.NeedTypedInvoke(ctx, sp, targetMethod, opCode == Constants.ECALL_NEWOBJ);
			}
			bool flag2 = typedInvoke;
			if (flag2)
			{
				this.InvokeTyped(ctx, targetMethod, opCode, ref sp, out state);
			}
			else
			{
				this.InvokeNormal(ctx, targetMethod, opCode, ref sp, out state);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003E44 File Offset: 0x00002044
		private void InvokeNormal(VMContext ctx, MethodBase targetMethod, byte opCode, ref uint sp, out ExecutionState state)
		{
			uint _sp = sp;
			ParameterInfo[] parameters = targetMethod.GetParameters();
			object self = null;
			object[] args = new object[parameters.Length];
			bool flag = opCode == Constants.ECALL_CALL && targetMethod.IsVirtual;
			if (flag)
			{
				int indexOffset = (targetMethod.IsStatic ? 0 : 1);
				args = new object[parameters.Length + indexOffset];
				for (int i = parameters.Length - 1; i >= 0; i--)
				{
					args[i + indexOffset] = Ecall.PopObject(ctx, parameters[i].ParameterType, ref sp);
				}
				bool flag2 = !targetMethod.IsStatic;
				if (flag2)
				{
					args[0] = Ecall.PopObject(ctx, targetMethod.DeclaringType, ref sp);
				}
				targetMethod = DirectCall.GetDirectInvocationProxy(targetMethod);
			}
			else
			{
				args = new object[parameters.Length];
				for (int j = parameters.Length - 1; j >= 0; j--)
				{
					args[j] = Ecall.PopObject(ctx, parameters[j].ParameterType, ref sp);
				}
				bool flag3 = !targetMethod.IsStatic && opCode != Constants.ECALL_NEWOBJ;
				if (flag3)
				{
					self = Ecall.PopObject(ctx, targetMethod.DeclaringType, ref sp);
					bool flag4 = self != null && !targetMethod.DeclaringType.IsInstanceOfType(self);
					if (flag4)
					{
						this.InvokeTyped(ctx, targetMethod, opCode, ref _sp, out state);
						return;
					}
				}
			}
			bool flag5 = opCode == Constants.ECALL_NEWOBJ;
			object result;
			if (flag5)
			{
				try
				{
					result = ((ConstructorInfo)targetMethod).Invoke(args);
				}
				catch (TargetInvocationException ex)
				{
					EHHelper.Rethrow(ex.InnerException, null);
					throw;
				}
			}
			else
			{
				bool flag6 = !targetMethod.IsStatic && self == null;
				if (flag6)
				{
					throw new NullReferenceException();
				}
				Type selfType;
				bool flag7 = self != null && (selfType = self.GetType()).IsArray && targetMethod.Name == "SetValue";
				if (flag7)
				{
					bool flag8 = args[0] == null;
					Type valueType;
					if (flag8)
					{
						valueType = selfType.GetElementType();
					}
					else
					{
						valueType = args[0].GetType();
					}
					ArrayStoreHelpers.SetValue((Array)self, (int)args[1], args[0], valueType, selfType.GetElementType());
					result = null;
				}
				else
				{
					try
					{
						result = targetMethod.Invoke(self, args);
					}
					catch (TargetInvocationException ex2)
					{
						VMDispatcher.DoThrow(ctx, ex2.InnerException);
						throw;
					}
				}
			}
			bool flag9 = targetMethod is MethodInfo && ((MethodInfo)targetMethod).ReturnType != typeof(void);
			if (flag9)
			{
				VMStack stack = ctx.Stack;
				uint num = sp + 1U;
				sp = num;
				stack[num] = VMSlot.FromObject(result, ((MethodInfo)targetMethod).ReturnType);
			}
			else
			{
				bool flag10 = opCode == Constants.ECALL_NEWOBJ;
				if (flag10)
				{
					VMStack stack2 = ctx.Stack;
					uint num = sp + 1U;
					sp = num;
					stack2[num] = VMSlot.FromObject(result, targetMethod.DeclaringType);
				}
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004160 File Offset: 0x00002360
		private void InvokeTyped(VMContext ctx, MethodBase targetMethod, byte opCode, ref uint sp, out ExecutionState state)
		{
			ParameterInfo[] parameters = targetMethod.GetParameters();
			int paramCount = parameters.Length;
			bool flag = !targetMethod.IsStatic && opCode != Constants.ECALL_NEWOBJ;
			if (flag)
			{
				paramCount++;
			}
			Type constrainType = null;
			bool flag2 = opCode == Constants.ECALL_CALLVIRT_CONSTRAINED;
			if (flag2)
			{
				VMData data = ctx.Instance.Data;
				VMStack stack = ctx.Stack;
				uint num = sp;
				sp = num - 1U;
				constrainType = (Type)data.LookupReference(stack[num].U4);
			}
			int num2 = ((targetMethod.IsStatic || opCode == Constants.ECALL_NEWOBJ) ? 0 : 1);
			IReference[] references = new IReference[paramCount];
			Type[] types = new Type[paramCount];
			for (int i = paramCount - 1; i >= 0; i--)
			{
				bool flag3 = !targetMethod.IsStatic && opCode != Constants.ECALL_NEWOBJ;
				Type paramType;
				if (flag3)
				{
					bool flag4 = i == 0;
					if (flag4)
					{
						bool flag5 = !targetMethod.IsStatic;
						if (flag5)
						{
							VMSlot thisSlot = ctx.Stack[sp];
							bool flag6 = thisSlot.O is ValueType && !targetMethod.DeclaringType.IsValueType;
							if (flag6)
							{
								Debug.Assert(targetMethod.DeclaringType.IsInterface);
								Debug.Assert(opCode == Constants.ECALL_CALLVIRT);
								constrainType = thisSlot.O.GetType();
							}
						}
						bool flag7 = constrainType != null;
						if (flag7)
						{
							paramType = constrainType.MakeByRefType();
						}
						else
						{
							bool isValueType = targetMethod.DeclaringType.IsValueType;
							if (isValueType)
							{
								paramType = targetMethod.DeclaringType.MakeByRefType();
							}
							else
							{
								paramType = targetMethod.DeclaringType;
							}
						}
					}
					else
					{
						paramType = parameters[i - 1].ParameterType;
					}
				}
				else
				{
					paramType = parameters[i].ParameterType;
				}
				references[i] = Ecall.PopRef(ctx, paramType, ref sp);
				bool isByRef = paramType.IsByRef;
				if (isByRef)
				{
					paramType = paramType.GetElementType();
				}
				types[i] = paramType;
			}
			bool flag8 = opCode == Constants.ECALL_CALL;
			OpCode callOp;
			Type retType;
			if (flag8)
			{
				callOp = OpCodes.Call;
				retType = ((targetMethod is MethodInfo) ? ((MethodInfo)targetMethod).ReturnType : typeof(void));
			}
			else
			{
				bool flag9 = opCode == Constants.ECALL_CALLVIRT || opCode == Constants.ECALL_CALLVIRT_CONSTRAINED;
				if (flag9)
				{
					callOp = OpCodes.Callvirt;
					retType = ((targetMethod is MethodInfo) ? ((MethodInfo)targetMethod).ReturnType : typeof(void));
				}
				else
				{
					bool flag10 = opCode == Constants.ECALL_NEWOBJ;
					if (!flag10)
					{
						throw new InvalidProgramException();
					}
					callOp = OpCodes.Newobj;
					retType = targetMethod.DeclaringType;
				}
			}
			DirectCall.TypedInvocation proxy = DirectCall.GetTypedInvocationProxy(targetMethod, callOp, constrainType);
			object result = proxy(ctx, references, types);
			bool flag11 = retType != typeof(void);
			if (flag11)
			{
				VMStack stack2 = ctx.Stack;
				uint num = sp + 1U;
				sp = num;
				stack2[num] = VMSlot.FromObject(result, retType);
			}
			else
			{
				bool flag12 = opCode == Constants.ECALL_NEWOBJ;
				if (flag12)
				{
					VMStack stack3 = ctx.Stack;
					uint num = sp + 1U;
					sp = num;
					stack3[num] = VMSlot.FromObject(result, retType);
				}
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
