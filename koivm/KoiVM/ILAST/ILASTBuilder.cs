using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.CFG;

namespace KoiVM.ILAST
{
	// Token: 0x0200010B RID: 267
	public class ILASTBuilder
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x000189A8 File Offset: 0x00016BA8
		private ILASTBuilder(MethodDef method, CilBody body, ScopeBlock scope)
		{
			this.method = method;
			this.body = body;
			this.scope = scope;
			this.basicBlocks = scope.GetBasicBlocks().Cast<BasicBlock<CILInstrList>>().ToList<BasicBlock<CILInstrList>>();
			this.blockHeaders = this.basicBlocks.ToDictionary((BasicBlock<CILInstrList> block) => block.Content[0], (BasicBlock<CILInstrList> block) => block);
			this.blockStates = new Dictionary<BasicBlock<CILInstrList>, ILASTBuilder.BlockState>();
			this.instrReferences = new List<ILASTExpression>();
			Debug.Assert(this.basicBlocks.Count > 0);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00018A64 File Offset: 0x00016C64
		public static void BuildAST(MethodDef method, CilBody body, ScopeBlock scope)
		{
			ILASTBuilder builder = new ILASTBuilder(method, body, scope);
			List<BasicBlock<CILInstrList>> basicBlocks = scope.GetBasicBlocks().Cast<BasicBlock<CILInstrList>>().ToList<BasicBlock<CILInstrList>>();
			builder.BuildAST();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00018A94 File Offset: 0x00016C94
		private void BuildAST()
		{
			this.BuildASTInternal();
			this.BuildPhiNodes();
			Dictionary<BasicBlock<CILInstrList>, BasicBlock<ILASTTree>> blockMap = this.scope.UpdateBasicBlocks<CILInstrList, ILASTTree>((BasicBlock<CILInstrList> block) => this.blockStates[block].ASTTree);
			Dictionary<Instruction, BasicBlock<ILASTTree>> newBlockMap = this.blockHeaders.ToDictionary((KeyValuePair<Instruction, BasicBlock<CILInstrList>> pair) => pair.Key, (KeyValuePair<Instruction, BasicBlock<CILInstrList>> pair) => blockMap[pair.Value]);
			Func<Instruction, IBasicBlock> <>9__3;
			foreach (ILASTExpression expr in this.instrReferences)
			{
				bool flag = expr.Operand is Instruction;
				if (flag)
				{
					expr.Operand = newBlockMap[(Instruction)expr.Operand];
				}
				else
				{
					ILASTExpression ilastexpression = expr;
					IEnumerable<Instruction> enumerable = (Instruction[])expr.Operand;
					Func<Instruction, IBasicBlock> func;
					if ((func = <>9__3) == null)
					{
						func = (<>9__3 = (Instruction instr) => newBlockMap[instr]);
					}
					ilastexpression.Operand = enumerable.Select(func).ToArray<IBasicBlock>();
				}
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00018BCC File Offset: 0x00016DCC
		private void BuildASTInternal()
		{
			Stack<BasicBlock<CILInstrList>> workList = new Stack<BasicBlock<CILInstrList>>();
			this.PopulateBeginStates(workList);
			HashSet<BasicBlock<CILInstrList>> visited = new HashSet<BasicBlock<CILInstrList>>();
			while (workList.Count > 0)
			{
				BasicBlock<CILInstrList> block = workList.Pop();
				bool flag = visited.Contains(block);
				if (!flag)
				{
					visited.Add(block);
					Debug.Assert(this.blockStates.ContainsKey(block));
					ILASTBuilder.BlockState state = this.blockStates[block];
					Debug.Assert(state.ASTTree == null);
					ILASTTree tree = this.BuildAST(block.Content, state.BeginStack);
					ILASTVariable[] remains = tree.StackRemains;
					state.ASTTree = tree;
					this.blockStates[block] = state;
					foreach (BasicBlock<CILInstrList> successor in block.Targets)
					{
						ILASTBuilder.BlockState successorState;
						bool flag2 = !this.blockStates.TryGetValue(successor, out successorState);
						if (flag2)
						{
							ILASTVariable[] blockVars = new ILASTVariable[remains.Length];
							for (int i = 0; i < blockVars.Length; i++)
							{
								blockVars[i] = new ILASTVariable
								{
									Name = string.Format("ph_{0:x2}_{1:x2}", successor.Id, i),
									Type = remains[i].Type,
									VariableType = ILASTVariableType.PhiVar
								};
							}
							successorState = new ILASTBuilder.BlockState
							{
								BeginStack = blockVars
							};
							this.blockStates[successor] = successorState;
						}
						else
						{
							bool flag3 = successorState.BeginStack.Length != remains.Length;
							if (flag3)
							{
								throw new InvalidProgramException("Inconsistent stack depth.");
							}
						}
						workList.Push(successor);
					}
				}
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00018DB4 File Offset: 0x00016FB4
		private void PopulateBeginStates(Stack<BasicBlock<CILInstrList>> workList)
		{
			for (int i = 0; i < this.body.ExceptionHandlers.Count; i++)
			{
				ExceptionHandler eh = this.body.ExceptionHandlers[i];
				this.blockStates[this.blockHeaders[eh.TryStart]] = new ILASTBuilder.BlockState
				{
					BeginStack = new ILASTVariable[0]
				};
				BasicBlock<CILInstrList> handlerBlock = this.blockHeaders[eh.HandlerStart];
				workList.Push(handlerBlock);
				bool flag = eh.HandlerType == ExceptionHandlerType.Fault || eh.HandlerType == ExceptionHandlerType.Finally;
				if (flag)
				{
					this.blockStates[handlerBlock] = new ILASTBuilder.BlockState
					{
						BeginStack = new ILASTVariable[0]
					};
				}
				else
				{
					ASTType type = TypeInference.ToASTType(eh.CatchType.ToTypeSig(true));
					bool flag2 = !this.blockStates.ContainsKey(handlerBlock);
					if (flag2)
					{
						ILASTVariable exVar = new ILASTVariable
						{
							Name = string.Format("ex_{0:x2}", i),
							Type = type,
							VariableType = ILASTVariableType.ExceptionVar,
							Annotation = eh
						};
						this.blockStates[handlerBlock] = new ILASTBuilder.BlockState
						{
							BeginStack = new ILASTVariable[] { exVar }
						};
					}
					else
					{
						Debug.Assert(this.blockStates[handlerBlock].BeginStack.Length == 1);
					}
					bool flag3 = eh.FilterStart != null;
					if (flag3)
					{
						ILASTVariable filterVar = new ILASTVariable
						{
							Name = string.Format("ef_{0:x2}", i),
							Type = type,
							VariableType = ILASTVariableType.FilterVar,
							Annotation = eh
						};
						BasicBlock<CILInstrList> filterBlock = this.blockHeaders[eh.FilterStart];
						workList.Push(filterBlock);
						this.blockStates[filterBlock] = new ILASTBuilder.BlockState
						{
							BeginStack = new ILASTVariable[] { filterVar }
						};
					}
				}
			}
			this.blockStates[this.basicBlocks[0]] = new ILASTBuilder.BlockState
			{
				BeginStack = new ILASTVariable[0]
			};
			workList.Push(this.basicBlocks[0]);
			foreach (BasicBlock<CILInstrList> block in this.basicBlocks)
			{
				bool flag4 = block.Sources.Count > 0;
				if (!flag4)
				{
					bool flag5 = workList.Contains(block);
					if (!flag5)
					{
						this.blockStates[block] = new ILASTBuilder.BlockState
						{
							BeginStack = new ILASTVariable[0]
						};
						workList.Push(block);
					}
				}
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000190A0 File Offset: 0x000172A0
		private void BuildPhiNodes()
		{
			foreach (KeyValuePair<BasicBlock<CILInstrList>, ILASTBuilder.BlockState> statePair in this.blockStates)
			{
				BasicBlock<CILInstrList> block = statePair.Key;
				ILASTBuilder.BlockState state = statePair.Value;
				bool flag = block.Sources.Count == 0 && state.BeginStack.Length != 0;
				if (flag)
				{
					Debug.Assert(state.BeginStack.Length == 1);
					ILASTPhi phi = new ILASTPhi
					{
						Variable = state.BeginStack[0],
						SourceVariables = new ILASTVariable[] { state.BeginStack[0] }
					};
					state.ASTTree.Insert(0, phi);
				}
				else
				{
					bool flag2 = state.BeginStack.Length != 0;
					if (flag2)
					{
						for (int varIndex = 0; varIndex < state.BeginStack.Length; varIndex++)
						{
							ILASTPhi phi2 = new ILASTPhi
							{
								Variable = state.BeginStack[varIndex]
							};
							phi2.SourceVariables = new ILASTVariable[block.Sources.Count];
							for (int i = 0; i < phi2.SourceVariables.Length; i++)
							{
								phi2.SourceVariables[i] = this.blockStates[block.Sources[i]].ASTTree.StackRemains[varIndex];
							}
							state.ASTTree.Insert(0, phi2);
						}
					}
				}
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00019258 File Offset: 0x00017458
		private ILASTTree BuildAST(CILInstrList instrs, ILASTVariable[] beginStack)
		{
			ILASTTree tree = new ILASTTree();
			Stack<ILASTVariable> evalStack = new Stack<ILASTVariable>(beginStack);
			Func<int, IILASTNode[]> popArgs = delegate(int numArgs)
			{
				IILASTNode[] args = new IILASTNode[numArgs];
				for (int i = numArgs - 1; i >= 0; i--)
				{
					args[i] = evalStack.Pop();
				}
				return args;
			};
			List<Instruction> prefixes = new List<Instruction>();
			foreach (Instruction instr in instrs)
			{
				bool flag = instr.OpCode.OpCodeType == OpCodeType.Prefix;
				if (flag)
				{
					prefixes.Add(instr);
				}
				else
				{
					bool flag2 = instr.OpCode.Code == Code.Dup;
					int pushes;
					ILASTExpression expr;
					if (flag2)
					{
						int pops = (pushes = 1);
						ILASTVariable arg = evalStack.Peek();
						expr = new ILASTExpression
						{
							ILCode = Code.Dup,
							Operand = null,
							Arguments = new IILASTNode[] { arg }
						};
					}
					else
					{
						int pops;
						instr.CalculateStackUsage(this.method.ReturnType.ElementType != ElementType.Void, out pushes, out pops);
						Debug.Assert(pushes == 0 || pushes == 1);
						bool flag3 = pops == -1;
						if (flag3)
						{
							evalStack.Clear();
							pops = 0;
						}
						expr = new ILASTExpression
						{
							ILCode = instr.OpCode.Code,
							Operand = instr.Operand,
							Arguments = popArgs(pops)
						};
						bool flag4 = expr.Operand is Instruction || expr.Operand is Instruction[];
						if (flag4)
						{
							this.instrReferences.Add(expr);
						}
					}
					expr.CILInstr = instr;
					bool flag5 = prefixes.Count > 0;
					if (flag5)
					{
						expr.Prefixes = prefixes.ToArray();
						prefixes.Clear();
					}
					bool flag6 = pushes == 1;
					if (flag6)
					{
						ILASTVariable variable = new ILASTVariable
						{
							Name = string.Format("s_{0:x4}", instr.Offset),
							VariableType = ILASTVariableType.StackVar
						};
						evalStack.Push(variable);
						tree.Add(new ILASTAssignment
						{
							Variable = variable,
							Value = expr
						});
					}
					else
					{
						tree.Add(expr);
					}
				}
			}
			tree.StackRemains = evalStack.Reverse<ILASTVariable>().ToArray<ILASTVariable>();
			return tree;
		}

		// Token: 0x040001DD RID: 477
		private MethodDef method;

		// Token: 0x040001DE RID: 478
		private CilBody body;

		// Token: 0x040001DF RID: 479
		private ScopeBlock scope;

		// Token: 0x040001E0 RID: 480
		private IList<BasicBlock<CILInstrList>> basicBlocks;

		// Token: 0x040001E1 RID: 481
		private Dictionary<Instruction, BasicBlock<CILInstrList>> blockHeaders;

		// Token: 0x040001E2 RID: 482
		private Dictionary<BasicBlock<CILInstrList>, ILASTBuilder.BlockState> blockStates;

		// Token: 0x040001E3 RID: 483
		private List<ILASTExpression> instrReferences;

		// Token: 0x0200010C RID: 268
		private struct BlockState
		{
			// Token: 0x040001E4 RID: 484
			public ILASTVariable[] BeginStack;

			// Token: 0x040001E5 RID: 485
			public ILASTTree ASTTree;
		}
	}
}
