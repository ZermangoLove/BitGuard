using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.VM;

namespace KoiVM.VMIR.RegAlloc
{
	// Token: 0x02000030 RID: 48
	public class RegisterAllocator
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000026BC File Offset: 0x000008BC
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000026C4 File Offset: 0x000008C4
		public int LocalSize { get; set; }

		// Token: 0x06000106 RID: 262 RVA: 0x000026CD File Offset: 0x000008CD
		public RegisterAllocator(IRTransformer transformer)
		{
			this.transformer = transformer;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007430 File Offset: 0x00005630
		public void Initialize()
		{
			List<BasicBlock<IRInstrList>> blocks = this.transformer.RootScope.GetBasicBlocks().Cast<BasicBlock<IRInstrList>>().ToList<BasicBlock<IRInstrList>>();
			this.liveness = LivenessAnalysis.ComputeLiveness(blocks);
			HashSet<IRVariable> stackVars = new HashSet<IRVariable>();
			foreach (KeyValuePair<BasicBlock<IRInstrList>, BlockLiveness> blockLiveness in this.liveness)
			{
				foreach (IRInstruction instr in blockLiveness.Key.Content)
				{
					bool flag = instr.OpCode != IROpCode.__LEA;
					if (!flag)
					{
						IRVariable variable = (IRVariable)instr.Operand2;
						bool flag2 = variable.VariableType != IRVariableType.Argument;
						if (flag2)
						{
							stackVars.Add(variable);
						}
					}
				}
				stackVars.UnionWith(blockLiveness.Value.OutLive);
			}
			int offset = 1;
			this.globalVars = stackVars.ToDictionary((IRVariable var) => var, delegate(IRVariable var)
			{
				int offset2 = offset;
				offset = offset2 + 1;
				return new RegisterAllocator.StackSlot(offset2, var);
			});
			this.baseOffset = offset;
			this.LocalSize = this.baseOffset - 1;
			offset = -2;
			IRVariable[] parameters = this.transformer.Context.GetParameters();
			for (int i = parameters.Length - 1; i >= 0; i--)
			{
				IRVariable paramVar = parameters[i];
				Dictionary<IRVariable, RegisterAllocator.StackSlot> dictionary = this.globalVars;
				IRVariable irvariable = paramVar;
				int offset3 = offset;
				offset = offset3 - 1;
				dictionary[irvariable] = new RegisterAllocator.StackSlot(offset3, paramVar);
			}
			this.allocation = this.globalVars.ToDictionary((KeyValuePair<IRVariable, RegisterAllocator.StackSlot> pair) => pair.Key, (KeyValuePair<IRVariable, RegisterAllocator.StackSlot> pair) => pair.Value);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007668 File Offset: 0x00005868
		public void Allocate(BasicBlock<IRInstrList> block)
		{
			BlockLiveness blockLiveness = this.liveness[block];
			Dictionary<IRInstruction, HashSet<IRVariable>> instrLiveness = LivenessAnalysis.ComputeLiveness(block, blockLiveness);
			RegisterAllocator.RegisterPool pool = RegisterAllocator.RegisterPool.Create(this.baseOffset, this.globalVars);
			for (int i = 0; i < block.Content.Count; i++)
			{
				IRInstruction instr = block.Content[i];
				pool.CheckLiveness(instrLiveness[instr]);
				bool flag = instr.Operand1 != null;
				if (flag)
				{
					instr.Operand1 = this.AllocateOperand(instr.Operand1, pool);
				}
				bool flag2 = instr.Operand2 != null;
				if (flag2)
				{
					instr.Operand2 = this.AllocateOperand(instr.Operand2, pool);
				}
			}
			bool flag3 = pool.SpillOffset - 1 > this.LocalSize;
			if (flag3)
			{
				this.LocalSize = pool.SpillOffset - 1;
			}
			this.baseOffset = pool.SpillOffset;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007758 File Offset: 0x00005958
		private IIROperand AllocateOperand(IIROperand operand, RegisterAllocator.RegisterPool pool)
		{
			bool flag = operand is IRVariable;
			IIROperand iiroperand;
			if (flag)
			{
				IRVariable variable = (IRVariable)operand;
				RegisterAllocator.StackSlot? slot;
				VMRegisters? reg = this.AllocateVariable(pool, variable, out slot);
				bool flag2 = reg != null;
				if (flag2)
				{
					iiroperand = new IRRegister(reg.Value)
					{
						SourceVariable = variable,
						Type = variable.Type
					};
				}
				else
				{
					variable.Annotation = slot.Value;
					iiroperand = new IRPointer
					{
						Register = IRRegister.BP,
						Offset = slot.Value.Offset,
						SourceVariable = variable,
						Type = variable.Type
					};
				}
			}
			else
			{
				iiroperand = operand;
			}
			return iiroperand;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007814 File Offset: 0x00005A14
		private VMRegisters? AllocateVariable(RegisterAllocator.RegisterPool pool, IRVariable var, out RegisterAllocator.StackSlot? stackSlot)
		{
			stackSlot = pool.CheckSpill(var);
			bool flag = stackSlot == null;
			if (flag)
			{
				VMRegisters? allocReg = ((var.Annotation == null) ? null : new VMRegisters?((VMRegisters)var.Annotation));
				bool flag2 = allocReg == null;
				if (flag2)
				{
					allocReg = pool.Allocate(var);
				}
				bool flag3 = allocReg != null;
				if (flag3)
				{
					bool flag4 = var.Annotation == null;
					if (flag4)
					{
						var.Annotation = allocReg.Value;
					}
					return allocReg;
				}
				stackSlot = new RegisterAllocator.StackSlot?(pool.SpillVariable(var));
			}
			return null;
		}

		// Token: 0x040000D5 RID: 213
		private IRTransformer transformer;

		// Token: 0x040000D6 RID: 214
		private Dictionary<BasicBlock<IRInstrList>, BlockLiveness> liveness;

		// Token: 0x040000D7 RID: 215
		private Dictionary<IRVariable, RegisterAllocator.StackSlot> globalVars;

		// Token: 0x040000D8 RID: 216
		private Dictionary<IRVariable, object> allocation;

		// Token: 0x040000D9 RID: 217
		private int baseOffset;

		// Token: 0x02000031 RID: 49
		private struct StackSlot
		{
			// Token: 0x0600010B RID: 267 RVA: 0x000026DE File Offset: 0x000008DE
			public StackSlot(int offset, IRVariable var)
			{
				this.Offset = offset;
				this.Variable = var;
			}

			// Token: 0x040000DB RID: 219
			public readonly int Offset;

			// Token: 0x040000DC RID: 220
			public readonly IRVariable Variable;
		}

		// Token: 0x02000032 RID: 50
		private class RegisterPool
		{
			// Token: 0x0600010C RID: 268 RVA: 0x000078D4 File Offset: 0x00005AD4
			private static VMRegisters ToRegister(int regId)
			{
				return (VMRegisters)regId;
			}

			// Token: 0x0600010D RID: 269 RVA: 0x000078E8 File Offset: 0x00005AE8
			private static int FromRegister(VMRegisters reg)
			{
				return (int)reg;
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x0600010E RID: 270 RVA: 0x000026EF File Offset: 0x000008EF
			// (set) Token: 0x0600010F RID: 271 RVA: 0x000026F7 File Offset: 0x000008F7
			public int SpillOffset { get; set; }

			// Token: 0x06000110 RID: 272 RVA: 0x000078FC File Offset: 0x00005AFC
			public static RegisterAllocator.RegisterPool Create(int baseOffset, Dictionary<IRVariable, RegisterAllocator.StackSlot> globalVars)
			{
				return new RegisterAllocator.RegisterPool
				{
					regAlloc = new IRVariable[8],
					spillVars = new Dictionary<IRVariable, RegisterAllocator.StackSlot>(globalVars),
					SpillOffset = baseOffset
				};
			}

			// Token: 0x06000111 RID: 273 RVA: 0x00007938 File Offset: 0x00005B38
			public VMRegisters? Allocate(IRVariable var)
			{
				for (int i = 0; i < this.regAlloc.Length; i++)
				{
					bool flag = this.regAlloc[i] == null;
					if (flag)
					{
						this.regAlloc[i] = var;
						return new VMRegisters?(RegisterAllocator.RegisterPool.ToRegister(i));
					}
				}
				return null;
			}

			// Token: 0x06000112 RID: 274 RVA: 0x00002700 File Offset: 0x00000900
			public void Deallocate(IRVariable var, VMRegisters reg)
			{
				Debug.Assert(this.regAlloc[RegisterAllocator.RegisterPool.FromRegister(reg)] == var);
				this.regAlloc[RegisterAllocator.RegisterPool.FromRegister(reg)] = null;
			}

			// Token: 0x06000113 RID: 275 RVA: 0x00007994 File Offset: 0x00005B94
			public void CheckLiveness(HashSet<IRVariable> live)
			{
				for (int i = 0; i < this.regAlloc.Length; i++)
				{
					bool flag = this.regAlloc[i] != null && !live.Contains(this.regAlloc[i]);
					if (flag)
					{
						this.regAlloc[i].Annotation = null;
						this.regAlloc[i] = null;
					}
				}
			}

			// Token: 0x06000114 RID: 276 RVA: 0x000079F8 File Offset: 0x00005BF8
			public RegisterAllocator.StackSlot SpillVariable(IRVariable var)
			{
				int spillOffset = this.SpillOffset;
				this.SpillOffset = spillOffset + 1;
				RegisterAllocator.StackSlot slot = new RegisterAllocator.StackSlot(spillOffset, var);
				this.spillVars[var] = slot;
				return slot;
			}

			// Token: 0x06000115 RID: 277 RVA: 0x00007A34 File Offset: 0x00005C34
			public RegisterAllocator.StackSlot? CheckSpill(IRVariable var)
			{
				RegisterAllocator.StackSlot ret;
				bool flag = !this.spillVars.TryGetValue(var, out ret);
				RegisterAllocator.StackSlot? stackSlot;
				if (flag)
				{
					stackSlot = null;
				}
				else
				{
					stackSlot = new RegisterAllocator.StackSlot?(ret);
				}
				return stackSlot;
			}

			// Token: 0x040000DD RID: 221
			private const int NumRegisters = 8;

			// Token: 0x040000DE RID: 222
			private IRVariable[] regAlloc;

			// Token: 0x040000DF RID: 223
			private Dictionary<IRVariable, RegisterAllocator.StackSlot> spillVars;
		}
	}
}
