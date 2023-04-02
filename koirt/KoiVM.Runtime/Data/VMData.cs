using System;
using System.Collections.Generic;
using System.Reflection;

namespace KoiVM.Runtime.Data
{
	// Token: 0x02000076 RID: 118
	internal class VMData
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000BFE1 File Offset: 0x0000A1E1
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000BFE9 File Offset: 0x0000A1E9
		public Module Module { get; private set; }

		// Token: 0x0600019A RID: 410 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		public unsafe VMData(Module module, void* data)
		{
			bool flag = ((VMData.VMDAT_HEADER*)data)->MAGIC != 1752394086U;
			if (flag)
			{
				throw new InvalidProgramException();
			}
			this.references = new Dictionary<uint, VMData.RefInfo>();
			this.strings = new Dictionary<uint, string>();
			this.exports = new Dictionary<uint, VMExportInfo>();
			byte* ptr = (byte*)data + sizeof(VMData.VMDAT_HEADER);
			int i = 0;
			while ((long)i < (long)((ulong)((VMData.VMDAT_HEADER*)data)->MD_COUNT))
			{
				uint id = Utils.ReadCompressedUInt(ref ptr);
				int token = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
				this.references[id] = new VMData.RefInfo
				{
					module = module,
					token = token
				};
				i++;
			}
			int j = 0;
			while ((long)j < (long)((ulong)((VMData.VMDAT_HEADER*)data)->STR_COUNT))
			{
				uint id2 = Utils.ReadCompressedUInt(ref ptr);
				uint len = Utils.ReadCompressedUInt(ref ptr);
				this.strings[id2] = new string((char*)ptr, 0, (int)len);
				ptr += len << 1;
				j++;
			}
			int k = 0;
			while ((long)k < (long)((ulong)((VMData.VMDAT_HEADER*)data)->EXP_COUNT))
			{
				this.exports[Utils.ReadCompressedUInt(ref ptr)] = new VMExportInfo(ref ptr, module);
				k++;
			}
			this.KoiSection = (byte*)data;
			this.Module = module;
			VMData.moduleVMData[module] = this;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000C14C File Offset: 0x0000A34C
		public static VMData Instance(Module module)
		{
			Dictionary<Module, VMData> dictionary = VMData.moduleVMData;
			VMData data;
			lock (dictionary)
			{
				bool flag = !VMData.moduleVMData.TryGetValue(module, out data);
				if (flag)
				{
					data = (VMData.moduleVMData[module] = VMDataInitializer.GetData(module));
				}
			}
			return data;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public unsafe byte* KoiSection { get; set; }

		// Token: 0x0600019E RID: 414 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		public MemberInfo LookupReference(uint id)
		{
			return this.references[id].Member;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000C1EC File Offset: 0x0000A3EC
		public string LookupString(uint id)
		{
			bool flag = id == 0U;
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				text = this.strings[id];
			}
			return text;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C218 File Offset: 0x0000A418
		public VMExportInfo LookupExport(uint id)
		{
			return this.exports[id];
		}

		// Token: 0x040000C9 RID: 201
		private Dictionary<uint, VMData.RefInfo> references;

		// Token: 0x040000CA RID: 202
		private Dictionary<uint, string> strings;

		// Token: 0x040000CB RID: 203
		private Dictionary<uint, VMExportInfo> exports;

		// Token: 0x040000CC RID: 204
		private static Dictionary<Module, VMData> moduleVMData = new Dictionary<Module, VMData>();

		// Token: 0x02000087 RID: 135
		private struct VMDAT_HEADER
		{
			// Token: 0x040000E1 RID: 225
			public uint MAGIC;

			// Token: 0x040000E2 RID: 226
			public uint MD_COUNT;

			// Token: 0x040000E3 RID: 227
			public uint STR_COUNT;

			// Token: 0x040000E4 RID: 228
			public uint EXP_COUNT;
		}

		// Token: 0x02000088 RID: 136
		private class RefInfo
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
			public MemberInfo Member
			{
				get
				{
					MemberInfo memberInfo;
					if ((memberInfo = this.resolved) == null)
					{
						memberInfo = (this.resolved = this.module.ResolveMember(this.token));
					}
					return memberInfo;
				}
			}

			// Token: 0x040000E5 RID: 229
			public Module module;

			// Token: 0x040000E6 RID: 230
			public int token;

			// Token: 0x040000E7 RID: 231
			public MemberInfo resolved;
		}
	}
}
