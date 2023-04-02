using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace KoiVM.VM
{
	// Token: 0x0200001B RID: 27
	public class DataDescriptor
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00005C9C File Offset: 0x00003E9C
		public DataDescriptor(Random random)
		{
			this.strMap[""] = 1U;
			this.nextStrId = 2U;
			this.nextRefId = 1U;
			this.nextSigId = 1U;
			this.random = random;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005D2C File Offset: 0x00003F2C
		public uint GetId(IMemberRef memberRef)
		{
			uint ret;
			bool flag = !this.refMap.TryGetValue(memberRef, out ret);
			if (flag)
			{
				Dictionary<IMemberRef, uint> dictionary = this.refMap;
				uint num = this.nextRefId;
				this.nextRefId = num + 1U;
				ret = (dictionary[memberRef] = num);
			}
			return ret;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005D74 File Offset: 0x00003F74
		public void ReplaceReference(IMemberRef old, IMemberRef @new)
		{
			uint id;
			bool flag = !this.refMap.TryGetValue(old, out id);
			if (!flag)
			{
				this.refMap.Remove(old);
				this.refMap[@new] = id;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public uint GetId(string str)
		{
			uint ret;
			bool flag = !this.strMap.TryGetValue(str, out ret);
			if (flag)
			{
				Dictionary<string, uint> dictionary = this.strMap;
				uint num = this.nextStrId;
				this.nextStrId = num + 1U;
				ret = (dictionary[str] = num);
			}
			return ret;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005DFC File Offset: 0x00003FFC
		public uint GetId(ITypeDefOrRef declType, MethodSig methodSig)
		{
			uint ret;
			bool flag = !this.sigMap.TryGetValue(methodSig, out ret);
			if (flag)
			{
				uint num = this.nextSigId;
				this.nextSigId = num + 1U;
				uint id = num;
				ret = (this.sigMap[methodSig] = id);
				this.sigs.Add(new DataDescriptor.FuncSigDesc(id, declType, methodSig));
			}
			return ret;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005E60 File Offset: 0x00004060
		public uint GetExportId(MethodDef method)
		{
			uint ret;
			bool flag = !this.exportMap.TryGetValue(method, out ret);
			if (flag)
			{
				uint num = this.nextSigId;
				this.nextSigId = num + 1U;
				uint id = num;
				ret = (this.exportMap[method] = id);
				this.sigs.Add(new DataDescriptor.FuncSigDesc(id, method));
			}
			return ret;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005EC4 File Offset: 0x000040C4
		public VMMethodInfo LookupInfo(MethodDef method)
		{
			VMMethodInfo ret;
			bool flag = !this.methodInfos.TryGetValue(method, out ret);
			if (flag)
			{
				int i = this.random.Next();
				ret = new VMMethodInfo
				{
					EntryKey = (byte)i,
					ExitKey = (byte)(i >> 8)
				};
				this.methodInfos[method] = ret;
			}
			return ret;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002377 File Offset: 0x00000577
		public void SetInfo(MethodDef method, VMMethodInfo info)
		{
			this.methodInfos[method] = info;
		}

		// Token: 0x04000057 RID: 87
		internal Dictionary<IMemberRef, uint> refMap = new Dictionary<IMemberRef, uint>();

		// Token: 0x04000058 RID: 88
		internal Dictionary<string, uint> strMap = new Dictionary<string, uint>(StringComparer.Ordinal);

		// Token: 0x04000059 RID: 89
		private Dictionary<MethodDef, uint> exportMap = new Dictionary<MethodDef, uint>();

		// Token: 0x0400005A RID: 90
		private Dictionary<MethodSig, uint> sigMap = new Dictionary<MethodSig, uint>(SignatureEqualityComparer.Instance);

		// Token: 0x0400005B RID: 91
		internal List<DataDescriptor.FuncSigDesc> sigs = new List<DataDescriptor.FuncSigDesc>();

		// Token: 0x0400005C RID: 92
		private Dictionary<MethodDef, VMMethodInfo> methodInfos = new Dictionary<MethodDef, VMMethodInfo>();

		// Token: 0x0400005D RID: 93
		private uint nextRefId;

		// Token: 0x0400005E RID: 94
		private uint nextStrId;

		// Token: 0x0400005F RID: 95
		private uint nextSigId;

		// Token: 0x04000060 RID: 96
		private Random random;

		// Token: 0x0200001C RID: 28
		internal class FuncSigDesc
		{
			// Token: 0x0600008A RID: 138 RVA: 0x00002388 File Offset: 0x00000588
			public FuncSigDesc(uint id, MethodDef method)
			{
				this.Id = id;
				this.Method = method;
				this.DeclaringType = method.DeclaringType;
				this.Signature = method.MethodSig;
				this.FuncSig = new FuncSig();
			}

			// Token: 0x0600008B RID: 139 RVA: 0x000023C3 File Offset: 0x000005C3
			public FuncSigDesc(uint id, ITypeDefOrRef declType, MethodSig sig)
			{
				this.Id = id;
				this.Method = null;
				this.DeclaringType = declType;
				this.Signature = sig;
				this.FuncSig = new FuncSig();
			}

			// Token: 0x04000061 RID: 97
			public readonly uint Id;

			// Token: 0x04000062 RID: 98
			public readonly MethodDef Method;

			// Token: 0x04000063 RID: 99
			public readonly ITypeDefOrRef DeclaringType;

			// Token: 0x04000064 RID: 100
			public readonly MethodSig Signature;

			// Token: 0x04000065 RID: 101
			public readonly FuncSig FuncSig;
		}
	}
}
