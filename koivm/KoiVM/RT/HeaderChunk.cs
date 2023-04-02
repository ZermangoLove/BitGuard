using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using KoiVM.AST.IL;
using KoiVM.VM;

namespace KoiVM.RT
{
	// Token: 0x020000F0 RID: 240
	internal class HeaderChunk : IKoiChunk
	{
		// Token: 0x06000397 RID: 919 RVA: 0x00002FA7 File Offset: 0x000011A7
		public HeaderChunk(VMRuntime rt)
		{
			this.Length = this.ComputeLength(rt);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012FD0 File Offset: 0x000111D0
		private uint GetCodedLen(MDToken token)
		{
			Table table = token.Table;
			Table table2 = table;
			if (table2 <= Table.MemberRef)
			{
				switch (table2)
				{
				case Table.TypeRef:
				case Table.TypeDef:
				case Table.Field:
				case Table.Method:
					break;
				case Table.FieldPtr:
				case Table.MethodPtr:
					goto IL_58;
				default:
					if (table2 != Table.MemberRef)
					{
						goto IL_58;
					}
					break;
				}
			}
			else if (table2 != Table.TypeSpec && table2 != Table.MethodSpec)
			{
				goto IL_58;
			}
			return Utils.GetCompressedUIntLength(token.Rid << 3);
			IL_58:
			throw new NotSupportedException();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001303C File Offset: 0x0001123C
		private uint GetCodedToken(MDToken token)
		{
			Table table = token.Table;
			Table table2 = table;
			if (table2 <= Table.MemberRef)
			{
				switch (table2)
				{
				case Table.TypeRef:
					return (token.Rid << 3) | 2U;
				case Table.TypeDef:
					return (token.Rid << 3) | 1U;
				case Table.FieldPtr:
				case Table.MethodPtr:
					break;
				case Table.Field:
					return (token.Rid << 3) | 6U;
				case Table.Method:
					return (token.Rid << 3) | 5U;
				default:
					if (table2 == Table.MemberRef)
					{
						return (token.Rid << 3) | 4U;
					}
					break;
				}
			}
			else
			{
				if (table2 == Table.TypeSpec)
				{
					return (token.Rid << 3) | 3U;
				}
				if (table2 == Table.MethodSpec)
				{
					return (token.Rid << 3) | 7U;
				}
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000130FC File Offset: 0x000112FC
		private uint ComputeLength(VMRuntime rt)
		{
			uint len = 16U;
			foreach (KeyValuePair<IMemberRef, uint> reference in rt.Descriptor.Data.refMap)
			{
				len += Utils.GetCompressedUIntLength(reference.Value) + this.GetCodedLen(reference.Key.MDToken);
			}
			foreach (KeyValuePair<string, uint> str in rt.Descriptor.Data.strMap)
			{
				len += Utils.GetCompressedUIntLength(str.Value);
				len += Utils.GetCompressedUIntLength((uint)str.Key.Length);
				len += (uint)(str.Key.Length * 2);
			}
			foreach (DataDescriptor.FuncSigDesc sig in rt.Descriptor.Data.sigs)
			{
				len += Utils.GetCompressedUIntLength(sig.Id);
				len += 4U;
				bool flag = sig.Method != null;
				if (flag)
				{
					len += 4U;
				}
				uint paramCount = (uint)sig.FuncSig.ParamSigs.Length;
				len += 1U + Utils.GetCompressedUIntLength(paramCount);
				foreach (ITypeDefOrRef param in sig.FuncSig.ParamSigs)
				{
					len += this.GetCodedLen(param.MDToken);
				}
				len += this.GetCodedLen(sig.FuncSig.RetType.MDToken);
			}
			return len;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000132E8 File Offset: 0x000114E8
		internal void WriteData(VMRuntime rt)
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(1752394086U);
			writer.Write(rt.Descriptor.Data.refMap.Count);
			writer.Write(rt.Descriptor.Data.strMap.Count);
			writer.Write(rt.Descriptor.Data.sigs.Count);
			foreach (KeyValuePair<IMemberRef, uint> refer in rt.Descriptor.Data.refMap)
			{
				writer.WriteCompressedUInt(refer.Value);
				writer.WriteCompressedUInt(this.GetCodedToken(refer.Key.MDToken));
			}
			foreach (KeyValuePair<string, uint> str in rt.Descriptor.Data.strMap)
			{
				writer.WriteCompressedUInt(str.Value);
				writer.WriteCompressedUInt((uint)str.Key.Length);
				foreach (char chr in str.Key)
				{
					writer.Write((ushort)chr);
				}
			}
			foreach (DataDescriptor.FuncSigDesc sig in rt.Descriptor.Data.sigs)
			{
				writer.WriteCompressedUInt(sig.Id);
				bool flag = sig.Method != null;
				if (flag)
				{
					ILBlock entry = rt.methodMap[sig.Method].Item2;
					uint entryOffset = entry.Content[0].Offset;
					Debug.Assert(entryOffset > 0U);
					writer.Write(entryOffset);
					uint key = (uint)rt.Descriptor.Random.Next();
					key = (key << 8) | (uint)rt.Descriptor.Data.LookupInfo(sig.Method).EntryKey;
					writer.Write(key);
				}
				else
				{
					writer.Write(0U);
				}
				writer.Write(sig.FuncSig.Flags);
				writer.WriteCompressedUInt((uint)sig.FuncSig.ParamSigs.Length);
				foreach (ITypeDefOrRef paramType in sig.FuncSig.ParamSigs)
				{
					writer.WriteCompressedUInt(this.GetCodedToken(paramType.MDToken));
				}
				writer.WriteCompressedUInt(this.GetCodedToken(sig.FuncSig.RetType.MDToken));
			}
			this.data = stream.ToArray();
			Debug.Assert((long)this.data.Length == (long)((ulong)this.Length));
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00002FBF File Offset: 0x000011BF
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00002FC7 File Offset: 0x000011C7
		public uint Length { get; set; }

		// Token: 0x0600039E RID: 926 RVA: 0x0000227A File Offset: 0x0000047A
		public void OnOffsetComputed(uint offset)
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00013644 File Offset: 0x00011844
		public byte[] GetData()
		{
			return this.data;
		}

		// Token: 0x0400017E RID: 382
		private byte[] data;
	}
}
