using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Pdb;

namespace KoiVM.CFG
{
	// Token: 0x0200011E RID: 286
	public class BlockParser
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x0001B228 File Offset: 0x00019428
		public static ScopeBlock Parse(MethodDef method, CilBody body)
		{
			body.SimplifyMacros(method.Parameters);
			BlockParser.ExpandSequencePoints(body);
			HashSet<Instruction> headers;
			HashSet<Instruction> entries;
			BlockParser.FindHeaders(body, out headers, out entries);
			List<BasicBlock<CILInstrList>> blocks = BlockParser.SplitBlocks(body, headers, entries);
			BlockParser.LinkBlocks(blocks);
			return BlockParser.AssignScopes(body, blocks);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001B270 File Offset: 0x00019470
		private static void ExpandSequencePoints(CilBody body)
		{
			SequencePoint current = null;
			foreach (Instruction instr in body.Instructions)
			{
				bool flag = instr.SequencePoint != null;
				if (flag)
				{
					current = instr.SequencePoint;
				}
				else
				{
					instr.SequencePoint = current;
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001B2DC File Offset: 0x000194DC
		private static void FindHeaders(CilBody body, out HashSet<Instruction> headers, out HashSet<Instruction> entries)
		{
			headers = new HashSet<Instruction>();
			entries = new HashSet<Instruction>();
			foreach (ExceptionHandler eh in body.ExceptionHandlers)
			{
				headers.Add(eh.TryStart);
				bool flag = eh.TryEnd != null;
				if (flag)
				{
					headers.Add(eh.TryEnd);
				}
				headers.Add(eh.HandlerStart);
				entries.Add(eh.HandlerStart);
				bool flag2 = eh.HandlerEnd != null;
				if (flag2)
				{
					headers.Add(eh.HandlerEnd);
				}
				bool flag3 = eh.FilterStart != null;
				if (flag3)
				{
					headers.Add(eh.FilterStart);
					entries.Add(eh.FilterStart);
				}
			}
			IList<Instruction> instrs = body.Instructions;
			for (int i = 0; i < instrs.Count; i++)
			{
				Instruction instr = instrs[i];
				bool flag4 = instr.Operand is Instruction;
				if (flag4)
				{
					headers.Add((Instruction)instr.Operand);
					bool flag5 = i + 1 < body.Instructions.Count;
					if (flag5)
					{
						headers.Add(body.Instructions[i + 1]);
					}
				}
				else
				{
					bool flag6 = instr.Operand is Instruction[];
					if (flag6)
					{
						foreach (Instruction target in (Instruction[])instr.Operand)
						{
							headers.Add(target);
						}
						bool flag7 = i + 1 < body.Instructions.Count;
						if (flag7)
						{
							headers.Add(body.Instructions[i + 1]);
						}
					}
					else
					{
						bool flag8 = (instr.OpCode.FlowControl == FlowControl.Throw || instr.OpCode.FlowControl == FlowControl.Return) && i + 1 < body.Instructions.Count;
						if (flag8)
						{
							headers.Add(body.Instructions[i + 1]);
						}
					}
				}
			}
			bool flag9 = instrs.Count > 0;
			if (flag9)
			{
				headers.Add(instrs[0]);
				entries.Add(instrs[0]);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001B558 File Offset: 0x00019758
		private static List<BasicBlock<CILInstrList>> SplitBlocks(CilBody body, HashSet<Instruction> headers, HashSet<Instruction> entries)
		{
			int nextBlockId = 0;
			int currentBlockId = -1;
			Instruction currentBlockHdr = null;
			List<BasicBlock<CILInstrList>> blocks = new List<BasicBlock<CILInstrList>>();
			CILInstrList instrList = new CILInstrList();
			for (int i = 0; i < body.Instructions.Count; i++)
			{
				Instruction instr = body.Instructions[i];
				bool flag = headers.Contains(instr);
				if (flag)
				{
					bool flag2 = currentBlockHdr != null;
					if (flag2)
					{
						Instruction footer = body.Instructions[i - 1];
						Debug.Assert(instrList.Count > 0);
						blocks.Add(new BasicBlock<CILInstrList>(currentBlockId, instrList));
						instrList = new CILInstrList();
					}
					currentBlockId = nextBlockId++;
					currentBlockHdr = instr;
				}
				instrList.Add(instr);
			}
			bool flag3 = blocks.Count == 0 || blocks[blocks.Count - 1].Id != currentBlockId;
			if (flag3)
			{
				Instruction footer2 = body.Instructions[body.Instructions.Count - 1];
				Debug.Assert(instrList.Count > 0);
				blocks.Add(new BasicBlock<CILInstrList>(currentBlockId, instrList));
			}
			return blocks;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001B680 File Offset: 0x00019880
		private static void LinkBlocks(List<BasicBlock<CILInstrList>> blocks)
		{
			Instruction instr;
			Dictionary<Instruction, BasicBlock<CILInstrList>> instrMap = blocks.SelectMany((BasicBlock<CILInstrList> block) => block.Content.Select((Instruction instr) => new
			{
				Instr = instr,
				Block = block
			})).ToDictionary(instr => instr.Instr, instr => instr.Block);
			foreach (BasicBlock<CILInstrList> block2 in blocks)
			{
				using (List<Instruction>.Enumerator enumerator2 = block2.Content.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						instr = enumerator2.Current;
						bool flag = instr.Operand is Instruction;
						if (flag)
						{
							BasicBlock<CILInstrList> dstBlock = instrMap[(Instruction)instr.Operand];
							dstBlock.Sources.Add(block2);
							block2.Targets.Add(dstBlock);
						}
						else
						{
							bool flag2 = instr.Operand is Instruction[];
							if (flag2)
							{
								foreach (Instruction target in (Instruction[])instr.Operand)
								{
									BasicBlock<CILInstrList> dstBlock2 = instrMap[target];
									dstBlock2.Sources.Add(block2);
									block2.Targets.Add(dstBlock2);
								}
							}
						}
					}
				}
			}
			for (int i = 0; i < blocks.Count; i++)
			{
				Instruction footer = blocks[i].Content.Last<Instruction>();
				bool flag3 = footer.OpCode.FlowControl != FlowControl.Branch && footer.OpCode.FlowControl != FlowControl.Return && footer.OpCode.FlowControl != FlowControl.Throw && i + 1 < blocks.Count;
				if (flag3)
				{
					BasicBlock<CILInstrList> src = blocks[i];
					BasicBlock<CILInstrList> dst = blocks[i + 1];
					bool flag4 = !src.Targets.Contains(dst);
					if (flag4)
					{
						src.Targets.Add(dst);
						dst.Sources.Add(src);
						src.Content.Add(Instruction.Create(OpCodes.Br, dst.Content[0]));
					}
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001B934 File Offset: 0x00019B34
		private static ScopeBlock AssignScopes(CilBody body, List<BasicBlock<CILInstrList>> blocks)
		{
			Dictionary<ExceptionHandler, Tuple<ScopeBlock, ScopeBlock, ScopeBlock>> ehScopes = new Dictionary<ExceptionHandler, Tuple<ScopeBlock, ScopeBlock, ScopeBlock>>();
			foreach (ExceptionHandler eh in body.ExceptionHandlers)
			{
				ScopeBlock tryBlock = new ScopeBlock(ScopeType.Try, eh);
				ScopeBlock handlerBlock = new ScopeBlock(ScopeType.Handler, eh);
				bool flag = eh.FilterStart != null;
				if (flag)
				{
					ScopeBlock filterBlock = new ScopeBlock(ScopeType.Filter, eh);
					ehScopes[eh] = Tuple.Create<ScopeBlock, ScopeBlock, ScopeBlock>(tryBlock, handlerBlock, filterBlock);
				}
				else
				{
					ehScopes[eh] = Tuple.Create<ScopeBlock, ScopeBlock, ScopeBlock>(tryBlock, handlerBlock, null);
				}
			}
			ScopeBlock root = new ScopeBlock();
			Stack<ScopeBlock> scopeStack = new Stack<ScopeBlock>();
			scopeStack.Push(root);
			foreach (BasicBlock<CILInstrList> block in blocks)
			{
				Instruction header = block.Content[0];
				foreach (ExceptionHandler eh2 in body.ExceptionHandlers)
				{
					Tuple<ScopeBlock, ScopeBlock, ScopeBlock> ehScope = ehScopes[eh2];
					bool flag2 = header == eh2.TryEnd;
					if (flag2)
					{
						ScopeBlock pop = scopeStack.Pop();
						Debug.Assert(pop == ehScope.Item1);
					}
					bool flag3 = header == eh2.HandlerEnd;
					if (flag3)
					{
						ScopeBlock pop2 = scopeStack.Pop();
						Debug.Assert(pop2 == ehScope.Item2);
					}
					bool flag4 = eh2.FilterStart != null && header == eh2.HandlerStart;
					if (flag4)
					{
						Debug.Assert(scopeStack.Peek().Type == ScopeType.Filter);
						ScopeBlock pop3 = scopeStack.Pop();
						Debug.Assert(pop3 == ehScope.Item3);
					}
				}
				foreach (ExceptionHandler eh3 in body.ExceptionHandlers.Reverse<ExceptionHandler>())
				{
					Tuple<ScopeBlock, ScopeBlock, ScopeBlock> ehScope2 = ehScopes[eh3];
					ScopeBlock parent = ((scopeStack.Count > 0) ? scopeStack.Peek() : null);
					bool flag5 = header == eh3.TryStart;
					if (flag5)
					{
						bool flag6 = parent != null;
						if (flag6)
						{
							BlockParser.AddScopeBlock(parent, ehScope2.Item1);
						}
						scopeStack.Push(ehScope2.Item1);
					}
					bool flag7 = header == eh3.HandlerStart;
					if (flag7)
					{
						bool flag8 = parent != null;
						if (flag8)
						{
							BlockParser.AddScopeBlock(parent, ehScope2.Item2);
						}
						scopeStack.Push(ehScope2.Item2);
					}
					bool flag9 = header == eh3.FilterStart;
					if (flag9)
					{
						bool flag10 = parent != null;
						if (flag10)
						{
							BlockParser.AddScopeBlock(parent, ehScope2.Item3);
						}
						scopeStack.Push(ehScope2.Item3);
					}
				}
				ScopeBlock scope = scopeStack.Peek();
				BlockParser.AddBasicBlock(scope, block);
			}
			foreach (ExceptionHandler eh4 in body.ExceptionHandlers)
			{
				bool flag11 = eh4.TryEnd == null;
				if (flag11)
				{
					ScopeBlock pop4 = scopeStack.Pop();
					Debug.Assert(pop4 == ehScopes[eh4].Item1);
				}
				bool flag12 = eh4.HandlerEnd == null;
				if (flag12)
				{
					ScopeBlock pop5 = scopeStack.Pop();
					Debug.Assert(pop5 == ehScopes[eh4].Item2);
				}
			}
			Debug.Assert(scopeStack.Count == 1);
			BlockParser.Validate(root);
			return root;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001BD5C File Offset: 0x00019F5C
		private static void Validate(ScopeBlock scope)
		{
			scope.Validate();
			foreach (ScopeBlock child in scope.Children)
			{
				BlockParser.Validate(child);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		private static void AddScopeBlock(ScopeBlock block, ScopeBlock child)
		{
			bool flag = block.Content.Count > 0;
			if (flag)
			{
				ScopeBlock newScope = new ScopeBlock();
				foreach (IBasicBlock instrBlock in block.Content)
				{
					newScope.Content.Add(instrBlock);
				}
				block.Content.Clear();
				block.Children.Add(newScope);
			}
			block.Children.Add(child);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001BE4C File Offset: 0x0001A04C
		private static void AddBasicBlock(ScopeBlock block, BasicBlock<CILInstrList> child)
		{
			bool flag = block.Children.Count > 0;
			if (flag)
			{
				ScopeBlock last = block.Children.Last<ScopeBlock>();
				bool flag2 = last.Type > ScopeType.None;
				if (flag2)
				{
					last = new ScopeBlock();
					block.Children.Add(last);
				}
				block = last;
			}
			Debug.Assert(block.Children.Count == 0);
			block.Content.Add(child);
		}
	}
}
