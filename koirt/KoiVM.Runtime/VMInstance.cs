using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KoiVM.Runtime.Data;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime
{
	// Token: 0x02000005 RID: 5
	internal class VMInstance
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000022F4 File Offset: 0x000004F4
		public static VMInstance Instance(Module module)
		{
			bool flag = VMInstance.instances == null;
			if (flag)
			{
				VMInstance.instances = new Dictionary<Module, VMInstance>();
			}
			VMInstance inst;
			bool flag2 = !VMInstance.instances.TryGetValue(module, out inst);
			if (flag2)
			{
				inst = new VMInstance(VMData.Instance(module));
				VMInstance.instances[module] = inst;
				object obj = VMInstance.initLock;
				lock (obj)
				{
					bool flag3 = !VMInstance.initialized.ContainsKey(module);
					if (flag3)
					{
						inst.Initialize();
						VMInstance.initialized.Add(module, VMInstance.initialized.Count);
					}
				}
			}
			return inst;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023AC File Offset: 0x000005AC
		public static VMInstance Instance(int id)
		{
			foreach (KeyValuePair<Module, int> entry in VMInstance.initialized)
			{
				bool flag = entry.Value == id;
				if (flag)
				{
					return VMInstance.Instance(entry.Key);
				}
			}
			return null;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000241C File Offset: 0x0000061C
		public static int GetModuleId(Module module)
		{
			return VMInstance.initialized[module];
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002439 File Offset: 0x00000639
		private VMInstance(VMData data)
		{
			this.Data = data;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002456 File Offset: 0x00000656
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000245E File Offset: 0x0000065E
		public VMData Data { get; private set; }

		// Token: 0x06000011 RID: 17 RVA: 0x00002468 File Offset: 0x00000668
		private void Initialize()
		{
			VMExportInfo initFunc = this.Data.LookupExport((uint)Constants.HELPER_INIT);
			ulong codeAddr = this.Data.KoiSection + initFunc.CodeOffset;
			this.Run(codeAddr, initFunc.EntryKey, initFunc.Signature, new object[0]);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024B8 File Offset: 0x000006B8
		public object Run(uint id, object[] arguments)
		{
			VMExportInfo export = this.Data.LookupExport(id);
			ulong codeAddr = this.Data.KoiSection + export.CodeOffset;
			return this.Run(codeAddr, export.EntryKey, export.Signature, arguments);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002500 File Offset: 0x00000700
		public object Run(ulong codeAddr, uint key, uint sigId, object[] arguments)
		{
			VMFuncSig sig = this.Data.LookupExport(sigId).Signature;
			return this.Run(codeAddr, key, sig, arguments);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002530 File Offset: 0x00000730
		public unsafe void Run(uint id, void*[] typedRefs, void* retTypedRef)
		{
			VMExportInfo export = this.Data.LookupExport(id);
			ulong codeAddr = this.Data.KoiSection + export.CodeOffset;
			this.Run(codeAddr, export.EntryKey, export.Signature, typedRefs, retTypedRef);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002578 File Offset: 0x00000778
		public unsafe void Run(ulong codeAddr, uint key, uint sigId, void*[] typedRefs, void* retTypedRef)
		{
			VMFuncSig sig = this.Data.LookupExport(sigId).Signature;
			this.Run(codeAddr, key, sig, typedRefs, retTypedRef);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025A8 File Offset: 0x000007A8
		private object Run(ulong codeAddr, uint key, VMFuncSig sig, object[] arguments)
		{
			bool flag = this.currentCtx != null;
			if (flag)
			{
				this.ctxStack.Push(this.currentCtx);
			}
			this.currentCtx = new VMContext(this);
			object obj;
			try
			{
				Debug.Assert(sig.ParamTypes.Length == arguments.Length);
				this.currentCtx.Stack.SetTopPosition((uint)(arguments.Length + 1));
				uint i = 0U;
				while ((ulong)i < (ulong)((long)arguments.Length))
				{
					this.currentCtx.Stack[i + 1U] = VMSlot.FromObject(arguments[(int)i], sig.ParamTypes[(int)i]);
					i += 1U;
				}
				this.currentCtx.Stack[(uint)(arguments.Length + 1)] = new VMSlot
				{
					U8 = 1UL
				};
				this.currentCtx.Registers[(int)Constants.REG_K1] = new VMSlot
				{
					U4 = key
				};
				this.currentCtx.Registers[(int)Constants.REG_BP] = new VMSlot
				{
					U4 = 0U
				};
				this.currentCtx.Registers[(int)Constants.REG_SP] = new VMSlot
				{
					U4 = (uint)(arguments.Length + 1)
				};
				this.currentCtx.Registers[(int)Constants.REG_IP] = new VMSlot
				{
					U8 = codeAddr
				};
				VMDispatcher.Run(this.currentCtx);
				Debug.Assert(this.currentCtx.EHStack.Count == 0);
				object retVal = null;
				bool flag2 = sig.RetType != typeof(void);
				if (flag2)
				{
					VMSlot retSlot = this.currentCtx.Registers[(int)Constants.REG_R0];
					bool flag3 = Type.GetTypeCode(sig.RetType) == TypeCode.String && retSlot.O == null;
					if (flag3)
					{
						retVal = this.Data.LookupString(retSlot.U4);
					}
					else
					{
						retVal = retSlot.ToObject(sig.RetType);
					}
				}
				obj = retVal;
			}
			finally
			{
				this.currentCtx.Stack.FreeAllLocalloc();
				bool flag4 = this.ctxStack.Count > 0;
				if (flag4)
				{
					this.currentCtx = this.ctxStack.Pop();
				}
			}
			return obj;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002818 File Offset: 0x00000A18
		private unsafe void Run(ulong codeAddr, uint key, VMFuncSig sig, void*[] arguments, void* retTypedRef)
		{
			bool flag = this.currentCtx != null;
			if (flag)
			{
				this.ctxStack.Push(this.currentCtx);
			}
			this.currentCtx = new VMContext(this);
			try
			{
				Debug.Assert(sig.ParamTypes.Length == arguments.Length);
				this.currentCtx.Stack.SetTopPosition((uint)(arguments.Length + 1));
				uint i = 0U;
				while ((ulong)i < (ulong)((long)arguments.Length))
				{
					Type paramType = sig.ParamTypes[(int)i];
					bool isByRef = paramType.IsByRef;
					if (isByRef)
					{
						this.currentCtx.Stack[i + 1U] = new VMSlot
						{
							O = new TypedRef(arguments[(int)i])
						};
					}
					else
					{
						TypedReference typedRef = *(TypedReference*)arguments[(int)i];
						this.currentCtx.Stack[i + 1U] = VMSlot.FromObject(TypedReference.ToObject(typedRef), __reftype(typedRef));
					}
					i += 1U;
				}
				this.currentCtx.Stack[(uint)(arguments.Length + 1)] = new VMSlot
				{
					U8 = 1UL
				};
				this.currentCtx.Registers[(int)Constants.REG_K1] = new VMSlot
				{
					U4 = key
				};
				this.currentCtx.Registers[(int)Constants.REG_BP] = new VMSlot
				{
					U4 = 0U
				};
				this.currentCtx.Registers[(int)Constants.REG_SP] = new VMSlot
				{
					U4 = (uint)(arguments.Length + 1)
				};
				this.currentCtx.Registers[(int)Constants.REG_IP] = new VMSlot
				{
					U8 = codeAddr
				};
				VMDispatcher.Run(this.currentCtx);
				Debug.Assert(this.currentCtx.EHStack.Count == 0);
				bool flag2 = sig.RetType != typeof(void);
				if (flag2)
				{
					bool isByRef2 = sig.RetType.IsByRef;
					if (isByRef2)
					{
						object retRef = this.currentCtx.Registers[(int)Constants.REG_R0].O;
						bool flag3 = !(retRef is IReference);
						if (flag3)
						{
							throw new ExecutionEngineException();
						}
						((IReference)retRef).ToTypedReference(this.currentCtx, retTypedRef, sig.RetType.GetElementType());
					}
					else
					{
						VMSlot retSlot = this.currentCtx.Registers[(int)Constants.REG_R0];
						bool flag4 = Type.GetTypeCode(sig.RetType) == TypeCode.String && retSlot.O == null;
						object retVal;
						if (flag4)
						{
							retVal = this.Data.LookupString(retSlot.U4);
						}
						else
						{
							retVal = retSlot.ToObject(sig.RetType);
						}
						TypedReferenceHelpers.SetTypedRef(retVal, retTypedRef);
					}
				}
			}
			finally
			{
				this.currentCtx.Stack.FreeAllLocalloc();
				bool flag5 = this.ctxStack.Count > 0;
				if (flag5)
				{
					this.currentCtx = this.ctxStack.Pop();
				}
			}
		}

		// Token: 0x04000003 RID: 3
		[ThreadStatic]
		private static Dictionary<Module, VMInstance> instances;

		// Token: 0x04000004 RID: 4
		private static readonly object initLock = new object();

		// Token: 0x04000005 RID: 5
		private static Dictionary<Module, int> initialized = new Dictionary<Module, int>();

		// Token: 0x04000006 RID: 6
		private Stack<VMContext> ctxStack = new Stack<VMContext>();

		// Token: 0x04000007 RID: 7
		private VMContext currentCtx;
	}
}
