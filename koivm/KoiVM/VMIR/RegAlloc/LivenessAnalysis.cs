using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.RegAlloc
{
	// Token: 0x0200002C RID: 44
	public class LivenessAnalysis
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00006F00 File Offset: 0x00005100
		public static Dictionary<BasicBlock<IRInstrList>, BlockLiveness> ComputeLiveness(IList<BasicBlock<IRInstrList>> blocks)
		{
			Dictionary<BasicBlock<IRInstrList>, BlockLiveness> liveness = new Dictionary<BasicBlock<IRInstrList>, BlockLiveness>();
			List<BasicBlock<IRInstrList>> entryBlocks = blocks.Where((BasicBlock<IRInstrList> block) => block.Sources.Count == 0).ToList<BasicBlock<IRInstrList>>();
			List<BasicBlock<IRInstrList>> order = new List<BasicBlock<IRInstrList>>();
			HashSet<BasicBlock<IRInstrList>> visited = new HashSet<BasicBlock<IRInstrList>>();
			Action<BasicBlock<IRInstrList>> <>9__1;
			foreach (BasicBlock<IRInstrList> entry in entryBlocks)
			{
				BasicBlock<IRInstrList> basicBlock = entry;
				HashSet<BasicBlock<IRInstrList>> hashSet = visited;
				Action<BasicBlock<IRInstrList>> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(BasicBlock<IRInstrList> block)
					{
						order.Add(block);
					});
				}
				LivenessAnalysis.PostorderTraversal(basicBlock, hashSet, action);
			}
			bool worked = false;
			do
			{
				foreach (BasicBlock<IRInstrList> currentBlock in order)
				{
					BlockLiveness blockLiveness = BlockLiveness.Empty();
					foreach (BasicBlock<IRInstrList> successor in currentBlock.Targets)
					{
						BlockLiveness successorLiveness;
						bool flag = !liveness.TryGetValue(successor, out successorLiveness);
						if (!flag)
						{
							blockLiveness.OutLive.UnionWith(successorLiveness.InLive);
						}
					}
					HashSet<IRVariable> live = new HashSet<IRVariable>(blockLiveness.OutLive);
					for (int i = currentBlock.Content.Count - 1; i >= 0; i--)
					{
						IRInstruction instr = currentBlock.Content[i];
						LivenessAnalysis.ComputeInstrLiveness(instr, live);
					}
					blockLiveness.InLive.UnionWith(live);
					BlockLiveness prevLiveness;
					bool flag2 = !worked && liveness.TryGetValue(currentBlock, out prevLiveness);
					if (flag2)
					{
						worked = !prevLiveness.InLive.SetEquals(blockLiveness.InLive) || !prevLiveness.OutLive.SetEquals(blockLiveness.OutLive);
					}
					liveness[currentBlock] = blockLiveness;
				}
			}
			while (worked);
			return liveness;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007164 File Offset: 0x00005364
		public static Dictionary<IRInstruction, HashSet<IRVariable>> ComputeLiveness(BasicBlock<IRInstrList> block, BlockLiveness liveness)
		{
			Dictionary<IRInstruction, HashSet<IRVariable>> ret = new Dictionary<IRInstruction, HashSet<IRVariable>>();
			HashSet<IRVariable> live = new HashSet<IRVariable>(liveness.OutLive);
			for (int i = block.Content.Count - 1; i >= 0; i--)
			{
				IRInstruction instr = block.Content[i];
				LivenessAnalysis.ComputeInstrLiveness(instr, live);
				ret[instr] = new HashSet<IRVariable>(live);
			}
			Debug.Assert(live.SetEquals(liveness.InLive));
			return ret;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000071E4 File Offset: 0x000053E4
		private static void PostorderTraversal(BasicBlock<IRInstrList> block, HashSet<BasicBlock<IRInstrList>> visited, Action<BasicBlock<IRInstrList>> visitFunc)
		{
			visited.Add(block);
			foreach (BasicBlock<IRInstrList> successor in block.Targets)
			{
				bool flag = !visited.Contains(successor);
				if (flag)
				{
					LivenessAnalysis.PostorderTraversal(successor, visited, visitFunc);
				}
			}
			visitFunc(block);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007258 File Offset: 0x00005458
		private static void ComputeInstrLiveness(IRInstruction instr, HashSet<IRVariable> live)
		{
			LivenessAnalysis.LiveFlags flags;
			bool flag = !LivenessAnalysis.opCodeLiveness.TryGetValue(instr.OpCode, out flags);
			if (flag)
			{
				flags = (LivenessAnalysis.LiveFlags)0;
			}
			IRVariable op = instr.Operand1 as IRVariable;
			IRVariable op2 = instr.Operand2 as IRVariable;
			bool flag2 = (flags & LivenessAnalysis.LiveFlags.KILL1) != (LivenessAnalysis.LiveFlags)0 && op != null;
			if (flag2)
			{
				live.Remove(op);
			}
			bool flag3 = (flags & LivenessAnalysis.LiveFlags.KILL2) != (LivenessAnalysis.LiveFlags)0 && op2 != null;
			if (flag3)
			{
				live.Remove(op2);
			}
			bool flag4 = (flags & LivenessAnalysis.LiveFlags.GEN1) != (LivenessAnalysis.LiveFlags)0 && op != null;
			if (flag4)
			{
				live.Add(op);
			}
			bool flag5 = (flags & LivenessAnalysis.LiveFlags.GEN2) != (LivenessAnalysis.LiveFlags)0 && op2 != null;
			if (flag5)
			{
				live.Add(op2);
			}
		}

		// Token: 0x040000CB RID: 203
		private static readonly Dictionary<IROpCode, LivenessAnalysis.LiveFlags> opCodeLiveness = new Dictionary<IROpCode, LivenessAnalysis.LiveFlags>
		{
			{
				IROpCode.MOV,
				(LivenessAnalysis.LiveFlags)6
			},
			{
				IROpCode.POP,
				LivenessAnalysis.LiveFlags.KILL1
			},
			{
				IROpCode.PUSH,
				LivenessAnalysis.LiveFlags.GEN1
			},
			{
				IROpCode.CALL,
				(LivenessAnalysis.LiveFlags)9
			},
			{
				IROpCode.NOR,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.CMP,
				(LivenessAnalysis.LiveFlags)3
			},
			{
				IROpCode.JZ,
				LivenessAnalysis.LiveFlags.GEN2
			},
			{
				IROpCode.JNZ,
				LivenessAnalysis.LiveFlags.GEN2
			},
			{
				IROpCode.SWT,
				LivenessAnalysis.LiveFlags.GEN2
			},
			{
				IROpCode.ADD,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.SUB,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.MUL,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.DIV,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.REM,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.SHR,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.SHL,
				(LivenessAnalysis.LiveFlags)7
			},
			{
				IROpCode.FCONV,
				(LivenessAnalysis.LiveFlags)6
			},
			{
				IROpCode.ICONV,
				(LivenessAnalysis.LiveFlags)6
			},
			{
				IROpCode.SX,
				(LivenessAnalysis.LiveFlags)6
			},
			{
				IROpCode.VCALL,
				LivenessAnalysis.LiveFlags.GEN1
			},
			{
				IROpCode.TRY,
				(LivenessAnalysis.LiveFlags)3
			},
			{
				IROpCode.LEAVE,
				LivenessAnalysis.LiveFlags.GEN1
			},
			{
				IROpCode.__EHRET,
				LivenessAnalysis.LiveFlags.GEN1
			},
			{
				IROpCode.__LEA,
				(LivenessAnalysis.LiveFlags)6
			},
			{
				IROpCode.__LDOBJ,
				(LivenessAnalysis.LiveFlags)9
			},
			{
				IROpCode.__STOBJ,
				(LivenessAnalysis.LiveFlags)3
			},
			{
				IROpCode.__GEN,
				LivenessAnalysis.LiveFlags.GEN1
			},
			{
				IROpCode.__KILL,
				LivenessAnalysis.LiveFlags.KILL1
			}
		};

		// Token: 0x0200002D RID: 45
		private enum LiveFlags
		{
			// Token: 0x040000CD RID: 205
			GEN1 = 1,
			// Token: 0x040000CE RID: 206
			GEN2,
			// Token: 0x040000CF RID: 207
			KILL1 = 4,
			// Token: 0x040000D0 RID: 208
			KILL2 = 8
		}
	}
}
