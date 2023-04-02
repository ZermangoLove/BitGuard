using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace KVM
{
	// Token: 0x02000008 RID: 8
	public static class Utils
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000029E0 File Offset: 0x00000BE0
		public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, List<TValue>> self, TKey key, TValue value)
		{
			bool flag = key == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				throw new ArgumentNullException("key");
			}
			List<TValue> list;
			bool flag4 = !self.TryGetValue(key, out list);
			bool flag5 = flag4;
			bool flag6 = flag5;
			if (flag6)
			{
				list = (self[key] = new List<TValue>());
			}
			list.Add(value);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A44 File Offset: 0x00000C44
		public static StrongNameKey LoadSNKey(string path, string pass)
		{
			bool flag = pass != null;
			StrongNameKey strongNameKey;
			if (flag)
			{
				X509Certificate2 x509Certificate = new X509Certificate2();
				x509Certificate.Import(path, pass, X509KeyStorageFlags.Exportable);
				RSACryptoServiceProvider rsacryptoServiceProvider = x509Certificate.PrivateKey as RSACryptoServiceProvider;
				bool flag2 = rsacryptoServiceProvider == null;
				if (flag2)
				{
					throw new ArgumentException("RSA key does not present in the certificate.", "path");
				}
				strongNameKey = new StrongNameKey(rsacryptoServiceProvider.ExportCspBlob(true));
			}
			else
			{
				strongNameKey = new StrongNameKey(path);
			}
			return strongNameKey;
		}

		// Token: 0x0400000B RID: 11
		public static ModuleWriterOptions ModuleWriterOptions;

		// Token: 0x0400000C RID: 12
		public static ModuleWriterListener ModuleWriterListener;
	}
}
