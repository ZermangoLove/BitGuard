using System;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.VM;
using KoiVM.VMIL;

namespace KoiVM.RT
{
	// Token: 0x020000EC RID: 236
	internal class BasicBlockChunk : IKoiChunk
	{
		// Token: 0x06000386 RID: 902 RVA: 0x00002F0E File Offset: 0x0000110E
		public BasicBlockChunk(VMRuntime rt, MethodDef method, ILBlock block)
		{
			this.rt = rt;
			this.method = method;
			this.Block = block;
			this.Length = rt.serializer.ComputeLength(block);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00002F41 File Offset: 0x00001141
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00002F49 File Offset: 0x00001149
		public ILBlock Block { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00002F52 File Offset: 0x00001152
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00002F5A File Offset: 0x0000115A
		public uint Length { get; set; }

		// Token: 0x0600038B RID: 907 RVA: 0x00012980 File Offset: 0x00010B80
		public void OnOffsetComputed(uint offset)
		{
			uint len = this.rt.serializer.ComputeOffset(this.Block, offset);
			Debug.Assert(len - offset == this.Length);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000129B8 File Offset: 0x00010BB8
		public byte[] GetData()
		{
			MemoryStream stream = new MemoryStream();
			this.rt.serializer.WriteData(this.Block, new BinaryWriter(stream));
			return this.Encrypt(stream.ToArray());
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000129FC File Offset: 0x00010BFC
		private byte[] Encrypt(byte[] data)
		{
			VMBlockKey blockKey = this.rt.Descriptor.Data.LookupInfo(this.method).BlockKeys[this.Block];
			byte currentKey = blockKey.EntryKey;
			ILInstruction firstInstr = this.Block.Content[0];
			ILInstruction lastInstr = this.Block.Content[this.Block.Content.Count - 1];
			foreach (ILInstruction instr in this.Block.Content)
			{
				uint instrStart = instr.Offset - firstInstr.Offset;
				uint instrEnd = instrStart + this.rt.serializer.ComputeLength(instr);
				byte b = data[(int)instrStart];
				uint num = instrStart;
				data[(int)num] = data[(int)num] ^ currentKey;
				currentKey = currentKey * 7 + b;
				byte? fixupTarget = null;
				bool flag = instr.Annotation == InstrAnnotation.JUMP || instr == lastInstr;
				if (flag)
				{
					fixupTarget = new byte?(blockKey.ExitKey);
				}
				else
				{
					bool flag2 = instr.OpCode == ILOpCode.LEAVE;
					if (flag2)
					{
						ExceptionHandler eh = ((EHInfo)instr.Annotation).ExceptionHandler;
						bool flag3 = eh.HandlerType == ExceptionHandlerType.Finally;
						if (flag3)
						{
							fixupTarget = new byte?(blockKey.ExitKey);
						}
					}
					else
					{
						bool flag4 = instr.OpCode == ILOpCode.CALL;
						if (flag4)
						{
							InstrCallInfo callInfo = (InstrCallInfo)instr.Annotation;
							VMMethodInfo info = this.rt.Descriptor.Data.LookupInfo((MethodDef)callInfo.Method);
							fixupTarget = new byte?(info.EntryKey);
						}
					}
				}
				bool flag5 = fixupTarget != null;
				if (flag5)
				{
					byte fixup = BasicBlockChunk.CalculateFixupByte(fixupTarget.Value, data, (uint)currentKey, instrStart + 1U, instrEnd);
					data[(int)(instrStart + 1U)] = fixup;
				}
				for (uint i = instrStart + 1U; i < instrEnd; i += 1U)
				{
					byte b2 = data[(int)i];
					uint num2 = i;
					data[(int)num2] = data[(int)num2] ^ currentKey;
					currentKey = currentKey * 7 + b2;
				}
				bool flag6 = fixupTarget != null;
				if (flag6)
				{
					Debug.Assert(currentKey == fixupTarget.Value);
				}
				bool flag7 = instr.OpCode == ILOpCode.CALL;
				if (flag7)
				{
					InstrCallInfo callInfo2 = (InstrCallInfo)instr.Annotation;
					VMMethodInfo info2 = this.rt.Descriptor.Data.LookupInfo((MethodDef)callInfo2.Method);
					currentKey = info2.ExitKey;
				}
			}
			return data;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00012CBC File Offset: 0x00010EBC
		private static byte CalculateFixupByte(byte target, byte[] data, uint currentKey, uint rangeStart, uint rangeEnd)
		{
			byte fixupByte = target;
			for (uint i = rangeEnd - 1U; i > rangeStart; i -= 1U)
			{
				fixupByte = (fixupByte - data[(int)i]) * 183;
			}
			return fixupByte - (byte)(currentKey * 7U);
		}

		// Token: 0x0400016F RID: 367
		private VMRuntime rt;

		// Token: 0x04000170 RID: 368
		private MethodDef method;
	}
}
