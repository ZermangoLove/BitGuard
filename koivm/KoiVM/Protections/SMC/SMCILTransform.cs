using System;
using System.Linq;
using KoiVM.AST.IL;
using KoiVM.CFG;
using KoiVM.VMIL;

namespace KoiVM.Protections.SMC
{
	// Token: 0x02000109 RID: 265
	internal class SMCILTransform : ITransform
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x000185B0 File Offset: 0x000167B0
		public void Initialize(ILTransformer tr)
		{
			this.trampoline = null;
			tr.RootScope.ProcessBasicBlocks<ILInstrList>(delegate(BasicBlock<ILInstrList> b)
			{
				bool flag2 = b.Content.Any((ILInstruction instr) => instr.IR != null && instr.IR.Annotation == SMCBlock.AddressPart2);
				if (flag2)
				{
					this.trampoline = (ILBlock)b;
				}
			});
			bool flag = this.trampoline == null;
			if (!flag)
			{
				ScopeBlock scope = tr.RootScope.SearchBlock(this.trampoline).Last<ScopeBlock>();
				this.newTrampoline = new SMCBlock(this.trampoline.Id, this.trampoline.Content);
				scope.Content[scope.Content.IndexOf(this.trampoline)] = this.newTrampoline;
				this.adrKey = tr.VM.Random.Next();
				this.newTrampoline.Key = (byte)tr.VM.Random.Next();
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00018680 File Offset: 0x00016880
		public void Transform(ILTransformer tr)
		{
			bool flag = tr.Block.Targets.Contains(this.trampoline);
			if (flag)
			{
				tr.Block.Targets[tr.Block.Targets.IndexOf(this.trampoline)] = this.newTrampoline;
			}
			bool flag2 = tr.Block.Sources.Contains(this.trampoline);
			if (flag2)
			{
				tr.Block.Sources[tr.Block.Sources.IndexOf(this.trampoline)] = this.newTrampoline;
			}
			tr.Instructions.VisitInstrs<ILTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00018738 File Offset: 0x00016938
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
		{
			bool flag = instr.Operand is ILBlockTarget;
			if (flag)
			{
				ILBlockTarget target = (ILBlockTarget)instr.Operand;
				bool flag2 = target.Target == this.trampoline;
				if (flag2)
				{
					target.Target = this.newTrampoline;
				}
			}
			else
			{
				bool flag3 = instr.IR == null;
				if (flag3)
				{
					return;
				}
			}
			bool flag4 = instr.IR.Annotation == SMCBlock.CounterInit && instr.OpCode == ILOpCode.PUSHI_DWORD;
			if (flag4)
			{
				ILImmediate imm = (ILImmediate)instr.Operand;
				bool flag5 = (int)imm.Value == 251658241;
				if (flag5)
				{
					this.newTrampoline.CounterOperand = imm;
				}
			}
			else
			{
				bool flag6 = instr.IR.Annotation == SMCBlock.EncryptionKey && instr.OpCode == ILOpCode.PUSHI_DWORD;
				if (flag6)
				{
					ILImmediate imm2 = (ILImmediate)instr.Operand;
					bool flag7 = (int)imm2.Value == 251658242;
					if (flag7)
					{
						imm2.Value = (int)this.newTrampoline.Key;
					}
				}
				else
				{
					bool flag8 = instr.IR.Annotation == SMCBlock.AddressPart1 && instr.OpCode == ILOpCode.PUSHI_DWORD && instr.Operand is ILBlockTarget;
					if (flag8)
					{
						ILBlockTarget target2 = (ILBlockTarget)instr.Operand;
						ILInstruction relBase = new ILInstruction(ILOpCode.PUSHR_QWORD, ILRegister.IP, instr);
						instr.OpCode = ILOpCode.PUSHI_DWORD;
						instr.Operand = new SMCBlockRef(target2, relBase, (uint)this.adrKey);
						instrs.Replace(index, new ILInstruction[]
						{
							relBase,
							instr,
							new ILInstruction(ILOpCode.ADD_QWORD, null, instr)
						});
					}
					else
					{
						bool flag9 = instr.IR.Annotation == SMCBlock.AddressPart2 && instr.OpCode == ILOpCode.PUSHI_DWORD;
						if (flag9)
						{
							ILImmediate imm3 = (ILImmediate)instr.Operand;
							bool flag10 = (int)imm3.Value == 251658243;
							if (flag10)
							{
								imm3.Value = this.adrKey;
							}
						}
					}
				}
			}
		}

		// Token: 0x040001D8 RID: 472
		private ILBlock trampoline;

		// Token: 0x040001D9 RID: 473
		private SMCBlock newTrampoline;

		// Token: 0x040001DA RID: 474
		private int adrKey;
	}
}
