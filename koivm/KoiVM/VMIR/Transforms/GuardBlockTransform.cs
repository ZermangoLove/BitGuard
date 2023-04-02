using System;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000AD RID: 173
	public class GuardBlockTransform : ITransform
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		public void Initialize(IRTransformer tr)
		{
			int maxId = 0;
			BasicBlock<IRInstrList> entry = null;
			tr.RootScope.ProcessBasicBlocks<IRInstrList>(delegate(BasicBlock<IRInstrList> block)
			{
				int id = block.Id;
				block.Id = id + 1;
				bool flag = block.Id > maxId;
				if (flag)
				{
					maxId = block.Id;
				}
				bool flag2 = entry == null;
				if (flag2)
				{
					entry = block;
				}
			});
			this.prolog = new BasicBlock<IRInstrList>(0, new IRInstrList
			{
				new IRInstruction(IROpCode.__ENTRY),
				new IRInstruction(IROpCode.JMP, new IRBlockTarget(entry))
			});
			this.prolog.Targets.Add(entry);
			entry.Sources.Add(this.prolog);
			this.epilog = new BasicBlock<IRInstrList>(maxId + 1, new IRInstrList
			{
				new IRInstruction(IROpCode.__EXIT)
			});
			this.InsertProlog(tr.RootScope);
			this.InsertEpilog(tr.RootScope);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
		private void InsertProlog(ScopeBlock block)
		{
			bool flag = block.Children.Count > 0;
			if (flag)
			{
				bool flag2 = block.Children[0].Type == ScopeType.None;
				if (flag2)
				{
					this.InsertProlog(block.Children[0]);
				}
				else
				{
					ScopeBlock prologScope = new ScopeBlock();
					prologScope.Content.Add(this.prolog);
					block.Children.Insert(0, prologScope);
				}
			}
			else
			{
				block.Content.Insert(0, this.prolog);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F764 File Offset: 0x0000D964
		private void InsertEpilog(ScopeBlock block)
		{
			bool flag = block.Children.Count > 0;
			if (flag)
			{
				bool flag2 = block.Children[block.Children.Count - 1].Type == ScopeType.None;
				if (flag2)
				{
					this.InsertEpilog(block.Children[block.Children.Count - 1]);
				}
				else
				{
					ScopeBlock epilogScope = new ScopeBlock();
					epilogScope.Content.Add(this.epilog);
					block.Children.Insert(block.Children.Count, epilogScope);
				}
			}
			else
			{
				block.Content.Insert(block.Content.Count, this.epilog);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000028C5 File Offset: 0x00000AC5
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F820 File Offset: 0x0000DA20
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.RET;
			if (flag)
			{
				instrs.Replace(index, new IRInstruction[]
				{
					new IRInstruction(IROpCode.JMP, new IRBlockTarget(this.epilog))
				});
				bool flag2 = !tr.Block.Targets.Contains(this.epilog);
				if (flag2)
				{
					tr.Block.Targets.Add(this.epilog);
					this.epilog.Sources.Add(tr.Block);
				}
			}
		}

		// Token: 0x040000EE RID: 238
		private BasicBlock<IRInstrList> prolog;

		// Token: 0x040000EF RID: 239
		private BasicBlock<IRInstrList> epilog;
	}
}
