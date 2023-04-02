using System;
using System.IO;
using dnlib.DotNet.Pdb;
using KoiVM.AST.IL;
using KoiVM.AST.ILAST;

namespace KoiVM.RT
{
	// Token: 0x020000E9 RID: 233
	internal class BasicBlockSerializer
	{
		// Token: 0x06000375 RID: 885 RVA: 0x00002EA6 File Offset: 0x000010A6
		public BasicBlockSerializer(VMRuntime rt)
		{
			this.rt = rt;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000122F4 File Offset: 0x000104F4
		public uint ComputeLength(ILBlock block)
		{
			uint len = 0U;
			foreach (ILInstruction instr in block.Content)
			{
				len += this.ComputeLength(instr);
			}
			return len;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00012354 File Offset: 0x00010554
		public uint ComputeLength(ILInstruction instr)
		{
			uint len = 2U;
			bool flag = instr.Operand != null;
			if (flag)
			{
				bool flag2 = instr.Operand is ILRegister;
				if (flag2)
				{
					len += 1U;
				}
				else
				{
					bool flag3 = instr.Operand is ILImmediate;
					if (flag3)
					{
						object value = ((ILImmediate)instr.Operand).Value;
						bool flag4 = value is uint || value is int || value is float;
						if (flag4)
						{
							len += 4U;
						}
						else
						{
							bool flag5 = value is ulong || value is long || value is double;
							if (!flag5)
							{
								throw new NotSupportedException();
							}
							len += 8U;
						}
					}
					else
					{
						bool flag6 = instr.Operand is ILRelReference;
						if (!flag6)
						{
							throw new NotSupportedException();
						}
						len += 4U;
					}
				}
			}
			return len;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00012440 File Offset: 0x00010640
		public uint ComputeOffset(ILBlock block, uint offset)
		{
			foreach (ILInstruction instr in block.Content)
			{
				instr.Offset = offset;
				offset += 2U;
				bool flag = instr.Operand != null;
				if (flag)
				{
					bool flag2 = instr.Operand is ILRegister;
					if (flag2)
					{
						offset += 1U;
					}
					else
					{
						bool flag3 = instr.Operand is ILImmediate;
						if (flag3)
						{
							object value = ((ILImmediate)instr.Operand).Value;
							bool flag4 = value is uint || value is int || value is float;
							if (flag4)
							{
								offset += 4U;
							}
							else
							{
								bool flag5 = value is ulong || value is long || value is double;
								if (!flag5)
								{
									throw new NotSupportedException();
								}
								offset += 8U;
							}
						}
						else
						{
							bool flag6 = instr.Operand is ILRelReference;
							if (!flag6)
							{
								throw new NotSupportedException();
							}
							offset += 4U;
						}
					}
				}
			}
			return offset;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00012590 File Offset: 0x00010790
		private static bool Equals(SequencePoint a, SequencePoint b)
		{
			return a.Document.Url == b.Document.Url && a.StartLine == b.StartLine;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000125D0 File Offset: 0x000107D0
		public void WriteData(ILBlock block, BinaryWriter writer)
		{
			uint offset = 0U;
			SequencePoint prevSeq = null;
			uint prevOffset = 0U;
			foreach (ILInstruction instr in block.Content)
			{
				bool flag = this.rt.dbgWriter != null && instr.IR.ILAST is ILASTExpression;
				if (flag)
				{
					ILASTExpression expr = (ILASTExpression)instr.IR.ILAST;
					SequencePoint seq = ((expr.CILInstr == null) ? null : expr.CILInstr.SequencePoint);
					bool flag2 = seq != null && seq.StartLine != 16707566 && (prevSeq == null || !BasicBlockSerializer.Equals(seq, prevSeq));
					if (flag2)
					{
						bool flag3 = prevSeq != null;
						if (flag3)
						{
							uint len = offset - prevOffset;
							uint line = (uint)prevSeq.StartLine;
							string doc = prevSeq.Document.Url;
							this.rt.dbgWriter.AddSequencePoint(block, prevOffset, len, doc, line);
						}
						prevSeq = seq;
						prevOffset = offset;
					}
				}
				writer.Write(this.rt.Descriptor.Architecture.OpCodes[instr.OpCode]);
				writer.Write((byte)this.rt.Descriptor.Random.Next());
				offset += 2U;
				bool flag4 = instr.Operand != null;
				if (flag4)
				{
					bool flag5 = instr.Operand is ILRegister;
					if (flag5)
					{
						writer.Write(this.rt.Descriptor.Architecture.Registers[((ILRegister)instr.Operand).Register]);
						offset += 1U;
					}
					else
					{
						bool flag6 = instr.Operand is ILImmediate;
						if (!flag6)
						{
							throw new NotSupportedException();
						}
						object value = ((ILImmediate)instr.Operand).Value;
						bool flag7 = value is int;
						if (flag7)
						{
							writer.Write((int)value);
							offset += 4U;
						}
						else
						{
							bool flag8 = value is uint;
							if (flag8)
							{
								writer.Write((uint)value);
								offset += 4U;
							}
							else
							{
								bool flag9 = value is long;
								if (flag9)
								{
									writer.Write((long)value);
									offset += 8U;
								}
								else
								{
									bool flag10 = value is ulong;
									if (flag10)
									{
										writer.Write((ulong)value);
										offset += 8U;
									}
									else
									{
										bool flag11 = value is float;
										if (flag11)
										{
											writer.Write((float)value);
											offset += 4U;
										}
										else
										{
											bool flag12 = value is double;
											if (flag12)
											{
												writer.Write((double)value);
												offset += 8U;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag13 = prevSeq != null;
			if (flag13)
			{
				uint len2 = offset - prevOffset;
				uint line2 = (uint)prevSeq.StartLine;
				string doc2 = prevSeq.Document.Url;
				this.rt.dbgWriter.AddSequencePoint(block, prevOffset, len2, doc2, line2);
			}
		}

		// Token: 0x0400016A RID: 362
		private VMRuntime rt;
	}
}
