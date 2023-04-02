using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using KVM;

namespace KoiVM.Console
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020AC File Offset: 0x000002AC
		private static void Main(string[] args)
		{
			bool flag = args.Length == 1;
			bool flag2 = flag;
			if (flag2)
			{
				string directoryName = Path.GetDirectoryName(args[0]);
				ModuleDef moduleDef = ModuleDefMD.Load(args[0], null);
				Program.ExceuteKoi(args[0], directoryName + "//SpectreVirt//" + Path.GetFileNameWithoutExtension(moduleDef.Name) + "_Virted.exe", "", null);
			}
			bool flag3 = args.Length == 2;
			bool flag4 = flag3;
			if (flag4)
			{
				Program.ExceuteKoi(args[0], args[1], "", null);
			}
			bool flag5 = args.Length == 3;
			bool flag6 = flag5;
			if (flag6)
			{
				Program.ExceuteKoi(args[0], args[1], args[2], null);
			}
			bool flag7 = args.Length == 4;
			bool flag8 = flag7;
			if (flag8)
			{
				Program.ExceuteKoi(args[0], args[1], args[2], args[3]);
			}
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002180 File Offset: 0x00000380
		private static string Hash(byte[] hash)
		{
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(hash);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2").ToLower());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021DC File Offset: 0x000003DC
		private static void ExceuteKoi(string input, string outPath, string snPath, string snPass)
		{
			try
			{
				bool flag = !Directory.Exists(Path.GetDirectoryName(input) + "//SpectreVirt");
				if (flag)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(input) + "//SpectreVirt");
				}
				new KVMTask().Exceute(ModuleDefMD.Load(input, null), outPath, snPath, snPass);
				byte[] array = File.ReadAllBytes(outPath);
				string text = Program.Hash(array);
				byte[] bytes = Encoding.Default.GetBytes(text);
				List<byte> list = new List<byte>();
				list.AddRange(array);
				list.AddRange(bytes);
				File.WriteAllBytes(outPath, list.ToArray());
			}
			catch (Exception ex)
			{
			}
		}
	}
}
