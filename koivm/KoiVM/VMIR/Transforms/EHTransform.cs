using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A6 RID: 166
	public class EHTransform : ITransform
	{
		// Token: 0x06000279 RID: 633 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		public void Transform(IRTransformer tr)
		{
			this.thisScopes = tr.RootScope.SearchBlock(tr.Block);
			this.AddTryStart(tr);
			bool flag = this.thisScopes[this.thisScopes.Length - 1].Type == ScopeType.Handler;
			if (flag)
			{
				ScopeBlock tryScope = this.SearchForTry(tr.RootScope, this.thisScopes[this.thisScopes.Length - 1].ExceptionHandler);
				ScopeBlock[] scopes = tr.RootScope.SearchBlock(tryScope.GetBasicBlocks().First<IBasicBlock>());
				this.thisScopes = scopes.TakeWhile((ScopeBlock s) => s != tryScope).ToArray<ScopeBlock>();
			}
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		private void SearchForHandlers(ScopeBlock scope, ExceptionHandler eh, ref IBasicBlock handler, ref IBasicBlock filter)
		{
			bool flag = scope.ExceptionHandler == eh;
			if (flag)
			{
				bool flag2 = scope.Type == ScopeType.Handler;
				if (flag2)
				{
					handler = scope.GetBasicBlocks().First<IBasicBlock>();
				}
				else
				{
					bool flag3 = scope.Type == ScopeType.Filter;
					if (flag3)
					{
						filter = scope.GetBasicBlocks().First<IBasicBlock>();
					}
				}
			}
			foreach (ScopeBlock child in scope.Children)
			{
				this.SearchForHandlers(child, eh, ref handler, ref filter);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000EC54 File Offset: 0x0000CE54
		private void AddTryStart(IRTransformer tr)
		{
			List<IRInstruction> tryStartInstrs = new List<IRInstruction>();
			for (int i = 0; i < this.thisScopes.Length; i++)
			{
				ScopeBlock scope = this.thisScopes[i];
				bool flag = scope.Type != ScopeType.Try;
				if (!flag)
				{
					bool flag2 = scope.GetBasicBlocks().First<IBasicBlock>() != tr.Block;
					if (!flag2)
					{
						IBasicBlock handler = null;
						IBasicBlock filter = null;
						this.SearchForHandlers(tr.RootScope, scope.ExceptionHandler, ref handler, ref filter);
						Debug.Assert(handler != null && (scope.ExceptionHandler.HandlerType != ExceptionHandlerType.Filter || filter != null));
						tryStartInstrs.Add(new IRInstruction(IROpCode.PUSH, new IRBlockTarget(handler)));
						IIROperand tryOperand = null;
						bool flag3 = scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Catch;
						int ehType;
						if (flag3)
						{
							tryOperand = IRConstant.FromI4((int)tr.VM.Data.GetId(scope.ExceptionHandler.CatchType));
							ehType = (int)tr.VM.Runtime.RTFlags.EH_CATCH;
						}
						else
						{
							bool flag4 = scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Filter;
							if (flag4)
							{
								tryOperand = new IRBlockTarget(filter);
								ehType = (int)tr.VM.Runtime.RTFlags.EH_FILTER;
							}
							else
							{
								bool flag5 = scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Fault;
								if (flag5)
								{
									ehType = (int)tr.VM.Runtime.RTFlags.EH_FAULT;
								}
								else
								{
									bool flag6 = scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Finally;
									if (!flag6)
									{
										throw new InvalidProgramException();
									}
									ehType = (int)tr.VM.Runtime.RTFlags.EH_FINALLY;
								}
							}
						}
						tryStartInstrs.Add(new IRInstruction(IROpCode.TRY, IRConstant.FromI4(ehType), tryOperand)
						{
							Annotation = new EHInfo(scope.ExceptionHandler)
						});
					}
				}
			}
			tr.Instructions.InsertRange(0, tryStartInstrs);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000EE48 File Offset: 0x0000D048
		private ScopeBlock SearchForTry(ScopeBlock scope, ExceptionHandler eh)
		{
			bool flag = scope.ExceptionHandler == eh && scope.Type == ScopeType.Try;
			ScopeBlock scopeBlock;
			if (flag)
			{
				scopeBlock = scope;
			}
			else
			{
				foreach (ScopeBlock child in scope.Children)
				{
					ScopeBlock s = this.SearchForTry(child, eh);
					bool flag2 = s != null;
					if (flag2)
					{
						return s;
					}
				}
				scopeBlock = null;
			}
			return scopeBlock;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		private static ScopeBlock FindCommonAncestor(ScopeBlock[] a, ScopeBlock[] b)
		{
			ScopeBlock ret = null;
			int i = 0;
			while (i < a.Length && i < b.Length)
			{
				bool flag = a[i] == b[i];
				if (!flag)
				{
					break;
				}
				ret = a[i];
				i++;
			}
			return ret;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000EF18 File Offset: 0x0000D118
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode != IROpCode.__LEAVE;
			if (!flag)
			{
				ScopeBlock[] targetScopes = tr.RootScope.SearchBlock(((IRBlockTarget)instr.Operand1).Target);
				ScopeBlock escapeTarget = EHTransform.FindCommonAncestor(this.thisScopes, targetScopes);
				List<IRInstruction> leaveInstrs = new List<IRInstruction>();
				for (int i = this.thisScopes.Length - 1; i >= 0; i--)
				{
					bool flag2 = this.thisScopes[i] == escapeTarget;
					if (flag2)
					{
						break;
					}
					bool flag3 = this.thisScopes[i].Type != ScopeType.Try;
					if (!flag3)
					{
						IBasicBlock handler = null;
						IBasicBlock filter = null;
						this.SearchForHandlers(tr.RootScope, this.thisScopes[i].ExceptionHandler, ref handler, ref filter);
						bool flag4 = handler == null;
						if (flag4)
						{
							throw new InvalidProgramException();
						}
						leaveInstrs.Add(new IRInstruction(IROpCode.LEAVE, new IRBlockTarget(handler))
						{
							Annotation = new EHInfo(this.thisScopes[i].ExceptionHandler)
						});
					}
				}
				instr.OpCode = IROpCode.JMP;
				leaveInstrs.Add(instr);
				instrs.Replace(index, leaveInstrs);
			}
		}

		// Token: 0x040000E7 RID: 231
		private ScopeBlock[] thisScopes;
	}
}
