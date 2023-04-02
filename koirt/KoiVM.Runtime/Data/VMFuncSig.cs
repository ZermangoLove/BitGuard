using System;
using System.Reflection;

namespace KoiVM.Runtime.Data
{
	// Token: 0x02000079 RID: 121
	internal class VMFuncSig
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x0000C340 File Offset: 0x0000A540
		public unsafe VMFuncSig(ref byte* ptr, Module module)
		{
			this.module = module;
			byte* ptr2 = ptr;
			ptr = ptr2 + 1;
			this.Flags = *ptr2;
			this.paramToks = new int[Utils.ReadCompressedUInt(ref ptr)];
			for (int i = 0; i < this.paramToks.Length; i++)
			{
				this.paramToks[i] = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
			}
			this.retTok = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		public Type[] ParamTypes
		{
			get
			{
				bool flag = this.paramTypes != null;
				Type[] array;
				if (flag)
				{
					array = this.paramTypes;
				}
				else
				{
					Type[] p = new Type[this.paramToks.Length];
					for (int i = 0; i < p.Length; i++)
					{
						p[i] = this.module.ResolveType(this.paramToks[i]);
					}
					this.paramTypes = p;
					array = p;
				}
				return array;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000C428 File Offset: 0x0000A628
		public Type RetType
		{
			get
			{
				Type type;
				if ((type = this.retType) == null)
				{
					type = (this.retType = this.module.ResolveType(this.retTok));
				}
				return type;
			}
		}

		// Token: 0x040000D3 RID: 211
		private Module module;

		// Token: 0x040000D4 RID: 212
		private readonly int[] paramToks;

		// Token: 0x040000D5 RID: 213
		private readonly int retTok;

		// Token: 0x040000D6 RID: 214
		private Type[] paramTypes;

		// Token: 0x040000D7 RID: 215
		private Type retType;

		// Token: 0x040000D8 RID: 216
		public byte Flags;
	}
}
