using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using KoiVM.AST.IL;

namespace KoiVM.RT
{
	// Token: 0x020000ED RID: 237
	[Obfuscation(Exclude = false, Feature = "+koi;-ref proxy")]
	internal class DbgWriter
	{
		// Token: 0x0600038F RID: 911 RVA: 0x00012CFC File Offset: 0x00010EFC
		public void AddSequencePoint(ILBlock block, uint offset, uint len, string document, uint lineNum)
		{
			List<DbgWriter.DbgEntry> entryList;
			bool flag = !this.entries.TryGetValue(block, out entryList);
			if (flag)
			{
				entryList = (this.entries[block] = new List<DbgWriter.DbgEntry>());
			}
			entryList.Add(new DbgWriter.DbgEntry
			{
				offset = offset,
				len = len,
				document = document,
				lineNum = lineNum
			});
			this.documents.Add(document);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00012D78 File Offset: 0x00010F78
		public DbgWriter.DbgSerializer GetSerializer()
		{
			return new DbgWriter.DbgSerializer(this);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00012D90 File Offset: 0x00010F90
		public byte[] GetDbgInfo()
		{
			return this.dbgInfo;
		}

		// Token: 0x04000173 RID: 371
		private Dictionary<ILBlock, List<DbgWriter.DbgEntry>> entries = new Dictionary<ILBlock, List<DbgWriter.DbgEntry>>();

		// Token: 0x04000174 RID: 372
		private HashSet<string> documents = new HashSet<string>();

		// Token: 0x04000175 RID: 373
		private byte[] dbgInfo;

		// Token: 0x020000EE RID: 238
		private struct DbgEntry
		{
			// Token: 0x04000176 RID: 374
			public uint offset;

			// Token: 0x04000177 RID: 375
			public uint len;

			// Token: 0x04000178 RID: 376
			public string document;

			// Token: 0x04000179 RID: 377
			public uint lineNum;
		}

		// Token: 0x020000EF RID: 239
		internal class DbgSerializer : IDisposable
		{
			// Token: 0x06000393 RID: 915 RVA: 0x00012DA8 File Offset: 0x00010FA8
			internal DbgSerializer(DbgWriter dbg)
			{
				this.dbg = dbg;
				this.stream = new MemoryStream();
				AesManaged aes = new AesManaged();
				aes.IV = (aes.Key = Convert.FromBase64String("UkVwAyrARLAy4GmQLL860w=="));
				this.writer = new BinaryWriter(new DeflateStream(new CryptoStream(this.stream, aes.CreateEncryptor(), CryptoStreamMode.Write), CompressionMode.Compress));
				this.InitStream();
			}

			// Token: 0x06000394 RID: 916 RVA: 0x00012E1C File Offset: 0x0001101C
			private void InitStream()
			{
				this.docMap = new Dictionary<string, uint>();
				this.writer.Write(this.dbg.documents.Count);
				uint docId = 0U;
				foreach (string doc in this.dbg.documents)
				{
					this.writer.Write(doc);
					this.docMap[doc] = docId++;
				}
			}

			// Token: 0x06000395 RID: 917 RVA: 0x00012EBC File Offset: 0x000110BC
			public void WriteBlock(BasicBlockChunk chunk)
			{
				List<DbgWriter.DbgEntry> entryList;
				bool flag = chunk == null || !this.dbg.entries.TryGetValue(chunk.Block, out entryList) || chunk.Block.Content.Count == 0;
				if (!flag)
				{
					uint offset = chunk.Block.Content[0].Offset;
					foreach (DbgWriter.DbgEntry entry in entryList)
					{
						this.writer.Write(entry.offset + chunk.Block.Content[0].Offset);
						this.writer.Write(entry.len);
						this.writer.Write(this.docMap[entry.document]);
						this.writer.Write(entry.lineNum);
					}
				}
			}

			// Token: 0x06000396 RID: 918 RVA: 0x00002F82 File Offset: 0x00001182
			public void Dispose()
			{
				this.writer.Dispose();
				this.dbg.dbgInfo = this.stream.ToArray();
			}

			// Token: 0x0400017A RID: 378
			private DbgWriter dbg;

			// Token: 0x0400017B RID: 379
			private BinaryWriter writer;

			// Token: 0x0400017C RID: 380
			private MemoryStream stream;

			// Token: 0x0400017D RID: 381
			private Dictionary<string, uint> docMap;
		}
	}
}
