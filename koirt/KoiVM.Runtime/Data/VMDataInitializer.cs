using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace KoiVM.Runtime.Data
{
	// Token: 0x0200007A RID: 122
	internal class VMDataInitializer
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x0000C460 File Offset: 0x0000A660
		internal unsafe static VMData GetData(Module module)
		{
			bool flag = !Platform.LittleEndian;
			if (flag)
			{
				throw new PlatformNotSupportedException();
			}
			byte* moduleBase = (byte*)(void*)Marshal.GetHINSTANCE(module);
			string fqn = module.FullyQualifiedName;
			bool isFlat = fqn.Length > 0 && fqn[0] == '<';
			bool flag2 = isFlat;
			VMData vmdata;
			if (flag2)
			{
				vmdata = new VMData(module, VMDataInitializer.GetKoiStreamFlat(moduleBase));
			}
			else
			{
				vmdata = new VMData(module, VMDataInitializer.GetKoiStreamMapped(moduleBase));
			}
			return vmdata;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		private unsafe static void* GetKoiStreamMapped(byte* moduleBase)
		{
			byte* ptr = moduleBase + 60;
			ptr = moduleBase + *(uint*)ptr;
			ptr += 6;
			ushort sectNum = *(ushort*)ptr;
			ptr += 14;
			ushort optSize = *(ushort*)ptr;
			ptr = ptr + 4 + optSize;
			byte* mdDir = moduleBase + *(uint*)(ptr - 16);
			byte* mdHdr = moduleBase + *(uint*)(mdDir + 8);
			mdHdr += 12;
			mdHdr += *(uint*)mdHdr;
			mdHdr = (mdHdr + 7L) & -4L;
			mdHdr += 2;
			ushort numOfStream = (ushort)(*mdHdr);
			mdHdr += 2;
			StringBuilder streamName = new StringBuilder();
			for (int i = 0; i < (int)numOfStream; i++)
			{
				uint offset = *(uint*)mdHdr;
				uint len = *(uint*)(mdHdr + 4);
				mdHdr += 8;
				streamName.Length = 0;
				for (int ii = 0; ii < 8; ii++)
				{
					streamName.Append((char)(*(mdHdr++)));
					bool flag = *mdHdr == 0;
					if (flag)
					{
						mdHdr += 3;
						break;
					}
					streamName.Append((char)(*(mdHdr++)));
					bool flag2 = *mdHdr == 0;
					if (flag2)
					{
						mdHdr += 2;
						break;
					}
					streamName.Append((char)(*(mdHdr++)));
					bool flag3 = *mdHdr == 0;
					if (flag3)
					{
						mdHdr++;
						break;
					}
					streamName.Append((char)(*(mdHdr++)));
					bool flag4 = *mdHdr == 0;
					if (flag4)
					{
						break;
					}
				}
				bool flag5 = streamName.ToString() == "?";
				if (flag5)
				{
					return VMDataInitializer.AllocateKoi((void*)(moduleBase + *(uint*)(mdDir + 8) + offset), len);
				}
			}
			return null;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000C66C File Offset: 0x0000A86C
		private unsafe static void* GetKoiStreamFlat(byte* moduleBase)
		{
			byte* ptr = moduleBase + 60;
			ptr = moduleBase + *(uint*)ptr;
			ptr += 6;
			ushort sectNum = *(ushort*)ptr;
			ptr += 14;
			ushort optSize = *(ushort*)ptr;
			ptr = ptr + 4 + optSize;
			uint mdDir = *(uint*)(ptr - 16);
			uint[] vAdrs = new uint[(int)sectNum];
			uint[] vSizes = new uint[(int)sectNum];
			uint[] rAdrs = new uint[(int)sectNum];
			for (int i = 0; i < (int)sectNum; i++)
			{
				vAdrs[i] = *(uint*)(ptr + 12);
				vSizes[i] = *(uint*)(ptr + 8);
				rAdrs[i] = *(uint*)(ptr + 20);
				ptr += 40;
			}
			for (int j = 0; j < (int)sectNum; j++)
			{
				bool flag = vAdrs[j] <= mdDir && mdDir < vAdrs[j] + vSizes[j];
				if (flag)
				{
					mdDir = mdDir - vAdrs[j] + rAdrs[j];
					break;
				}
			}
			byte* mdDirPtr = moduleBase + mdDir;
			uint mdHdr = *(uint*)(mdDirPtr + 8);
			for (int k = 0; k < (int)sectNum; k++)
			{
				bool flag2 = vAdrs[k] <= mdHdr && mdHdr < vAdrs[k] + vSizes[k];
				if (flag2)
				{
					mdHdr = mdHdr - vAdrs[k] + rAdrs[k];
					break;
				}
			}
			byte* mdHdrPtr = moduleBase + mdHdr;
			mdHdrPtr += 12;
			mdHdrPtr += *(uint*)mdHdrPtr;
			mdHdrPtr = (mdHdrPtr + 7L) & -4L;
			mdHdrPtr += 2;
			ushort numOfStream = (ushort)(*mdHdrPtr);
			mdHdrPtr += 2;
			StringBuilder streamName = new StringBuilder();
			for (int l = 0; l < (int)numOfStream; l++)
			{
				uint offset = *(uint*)mdHdrPtr;
				uint len = *(uint*)(mdHdrPtr + 4);
				streamName.Length = 0;
				mdHdrPtr += 8;
				for (int ii = 0; ii < 8; ii++)
				{
					streamName.Append((char)(*(mdHdrPtr++)));
					bool flag3 = *mdHdrPtr == 0;
					if (flag3)
					{
						mdHdrPtr += 3;
						break;
					}
					streamName.Append((char)(*(mdHdrPtr++)));
					bool flag4 = *mdHdrPtr == 0;
					if (flag4)
					{
						mdHdrPtr += 2;
						break;
					}
					streamName.Append((char)(*(mdHdrPtr++)));
					bool flag5 = *mdHdrPtr == 0;
					if (flag5)
					{
						mdHdrPtr++;
						break;
					}
					streamName.Append((char)(*(mdHdrPtr++)));
					bool flag6 = *mdHdrPtr == 0;
					if (flag6)
					{
						break;
					}
				}
				bool flag7 = streamName.ToString() == "?";
				if (flag7)
				{
					return VMDataInitializer.AllocateKoi((void*)(moduleBase + mdHdr + offset), len);
				}
			}
			return null;
		}

		// Token: 0x060001AB RID: 427
		[DllImport("kernel32.dll")]
		private unsafe static extern void CopyMemory(void* dest, void* src, uint count);

		// Token: 0x060001AC RID: 428 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		private unsafe static void* AllocateKoi(void* ptr, uint len)
		{
			void* koi = (void*)Marshal.AllocHGlobal((int)len);
			VMDataInitializer.CopyMemory(koi, ptr, len);
			return koi;
		}
	}
}
