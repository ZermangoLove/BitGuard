using System;
using System.Collections.Generic;
using System.Reflection;
using KoiVM.Runtime.Data;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.VCalls
{
	// Token: 0x02000010 RID: 16
	internal class Ldftn : IVCall
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000341C File Offset: 0x0000161C
		public byte Code
		{
			get
			{
				return Constants.VCALL_LDFTN;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003434 File Offset: 0x00001634
		public void Run(VMContext ctx, out ExecutionState state)
		{
			uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
			VMSlot methodSlot = ctx.Stack[sp--];
			VMSlot objectSlot = ctx.Stack[sp];
			bool flag = objectSlot.O != null;
			if (flag)
			{
				MethodInfo method = (MethodInfo)ctx.Instance.Data.LookupReference(methodSlot.U4);
				Type type = objectSlot.O.GetType();
				List<Type> baseTypes = new List<Type>();
				do
				{
					baseTypes.Add(type);
					type = type.BaseType;
				}
				while (type != null && type != method.DeclaringType);
				baseTypes.Reverse();
				MethodInfo found = method;
				foreach (Type baseType in baseTypes)
				{
					foreach (MethodInfo i in baseType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						bool flag2 = i.GetBaseDefinition() == found;
						if (flag2)
						{
							found = i;
							break;
						}
					}
				}
				ctx.Stack[sp] = new VMSlot
				{
					U8 = (ulong)(long)found.MethodHandle.GetFunctionPointer()
				};
			}
			bool flag3 = objectSlot.U8 > 0UL;
			if (flag3)
			{
				uint entryKey = ctx.Stack[sp -= 1U].U4;
				ulong codeAdr = methodSlot.U8;
				VMFuncSig sig = ctx.Instance.Data.LookupExport(objectSlot.U4).Signature;
				IntPtr ptr = VMTrampoline.CreateTrampoline(ctx.Instance.Data.Module, codeAdr, entryKey, sig, objectSlot.U4);
				ctx.Stack[sp] = new VMSlot
				{
					U8 = (ulong)(long)ptr
				};
			}
			else
			{
				MethodBase method2 = (MethodBase)ctx.Instance.Data.LookupReference(methodSlot.U4);
				ctx.Stack[sp] = new VMSlot
				{
					U8 = (ulong)(long)method2.MethodHandle.GetFunctionPointer()
				};
			}
			ctx.Stack.SetTopPosition(sp);
			ctx.Registers[(int)Constants.REG_SP].U4 = sp;
			state = ExecutionState.Next;
		}
	}
}
