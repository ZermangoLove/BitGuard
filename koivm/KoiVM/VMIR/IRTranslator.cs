using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VM;

namespace KoiVM.VMIR
{
	// Token: 0x02000025 RID: 37
	public class IRTranslator
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x000067EC File Offset: 0x000049EC
		static IRTranslator()
		{
			foreach (Type type in typeof(IRTranslator).Assembly.GetExportedTypes())
			{
				bool flag = typeof(ITranslationHandler).IsAssignableFrom(type) && !type.IsAbstract;
				if (flag)
				{
					ITranslationHandler handler = (ITranslationHandler)Activator.CreateInstance(type);
					IRTranslator.handlers.Add(handler.ILCode, handler);
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000025E6 File Offset: 0x000007E6
		public IRTranslator(IRContext ctx, VMRuntime runtime)
		{
			this.Context = ctx;
			this.Runtime = runtime;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002600 File Offset: 0x00000800
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00002608 File Offset: 0x00000808
		public ScopeBlock RootScope { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002611 File Offset: 0x00000811
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002619 File Offset: 0x00000819
		public IRContext Context { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002622 File Offset: 0x00000822
		// (set) Token: 0x060000DF RID: 223 RVA: 0x0000262A File Offset: 0x0000082A
		public VMRuntime Runtime { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006874 File Offset: 0x00004A74
		public VMDescriptor VM
		{
			get
			{
				return this.Runtime.Descriptor;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006894 File Offset: 0x00004A94
		public ArchDescriptor Arch
		{
			get
			{
				return this.VM.Architecture;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00002633 File Offset: 0x00000833
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000263B File Offset: 0x0000083B
		internal BasicBlock<ILASTTree> Block { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00002644 File Offset: 0x00000844
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000264C File Offset: 0x0000084C
		internal IRInstrList Instructions { get; private set; }

		// Token: 0x060000E6 RID: 230 RVA: 0x000068B4 File Offset: 0x00004AB4
		internal IIROperand Translate(IILASTNode node)
		{
			bool flag = node is ILASTExpression;
			if (flag)
			{
				ILASTExpression expr = (ILASTExpression)node;
				try
				{
					ITranslationHandler handler;
					bool flag2 = !IRTranslator.handlers.TryGetValue(expr.ILCode, out handler);
					if (flag2)
					{
						throw new NotSupportedException(expr.ILCode.ToString());
					}
					int i = this.Instructions.Count;
					IIROperand operand = handler.Translate(expr, this);
					while (i < this.Instructions.Count)
					{
						this.Instructions[i].ILAST = expr;
						i++;
					}
					return operand;
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("Failed to translate expr {0} @ {1:x4}.", expr.CILInstr, expr.CILInstr.GetOffset()), ex);
				}
			}
			bool flag3 = node is ILASTVariable;
			if (!flag3)
			{
				throw new NotSupportedException();
			}
			return this.Context.ResolveVRegister((ILASTVariable)node);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000069C4 File Offset: 0x00004BC4
		private IRInstrList Translate(BasicBlock<ILASTTree> block)
		{
			this.Block = block;
			this.Instructions = new IRInstrList();
			bool seenJump = false;
			foreach (IILASTStatement st in block.Content)
			{
				bool flag = st is ILASTPhi;
				if (flag)
				{
					ILASTVariable variable = ((ILASTPhi)st).Variable;
					this.Instructions.Add(new IRInstruction(IROpCode.POP)
					{
						Operand1 = this.Context.ResolveVRegister(variable),
						ILAST = st
					});
				}
				else
				{
					bool flag2 = st is ILASTAssignment;
					if (flag2)
					{
						ILASTAssignment assignment = (ILASTAssignment)st;
						IIROperand valueVar = this.Translate(assignment.Value);
						this.Instructions.Add(new IRInstruction(IROpCode.MOV)
						{
							Operand1 = this.Context.ResolveVRegister(assignment.Variable),
							Operand2 = valueVar,
							ILAST = st
						});
					}
					else
					{
						bool flag3 = st is ILASTExpression;
						if (!flag3)
						{
							throw new NotSupportedException();
						}
						ILASTExpression expr = (ILASTExpression)st;
						OpCode opCode = expr.ILCode.ToOpCode();
						bool flag4 = !seenJump && (opCode.FlowControl == FlowControl.Cond_Branch || opCode.FlowControl == FlowControl.Branch || opCode.FlowControl == FlowControl.Return || opCode.FlowControl == FlowControl.Throw);
						if (flag4)
						{
							foreach (ILASTVariable remain in block.Content.StackRemains)
							{
								this.Instructions.Add(new IRInstruction(IROpCode.PUSH)
								{
									Operand1 = this.Context.ResolveVRegister(remain),
									ILAST = st
								});
							}
							seenJump = true;
						}
						this.Translate((ILASTExpression)st);
					}
				}
			}
			Debug.Assert(seenJump);
			IRInstrList ret = this.Instructions;
			this.Instructions = null;
			return ret;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006BE8 File Offset: 0x00004DE8
		public void Translate(ScopeBlock rootScope)
		{
			this.RootScope = rootScope;
			Dictionary<BasicBlock<ILASTTree>, BasicBlock<IRInstrList>> blockMap = rootScope.UpdateBasicBlocks<ILASTTree, IRInstrList>((BasicBlock<ILASTTree> block) => this.Translate(block));
			rootScope.ProcessBasicBlocks<IRInstrList>(delegate(BasicBlock<IRInstrList> block)
			{
				foreach (IRInstruction instr in block.Content)
				{
					bool flag = instr.Operand1 is IRBlockTarget;
					if (flag)
					{
						IRBlockTarget op = (IRBlockTarget)instr.Operand1;
						op.Target = blockMap[(BasicBlock<ILASTTree>)op.Target];
					}
					else
					{
						bool flag2 = instr.Operand1 is IRJumpTable;
						if (flag2)
						{
							IRJumpTable op2 = (IRJumpTable)instr.Operand1;
							for (int i = 0; i < op2.Targets.Length; i++)
							{
								op2.Targets[i] = blockMap[(BasicBlock<ILASTTree>)op2.Targets[i]];
							}
						}
					}
					bool flag3 = instr.Operand2 is IRBlockTarget;
					if (flag3)
					{
						IRBlockTarget op3 = (IRBlockTarget)instr.Operand2;
						op3.Target = blockMap[(BasicBlock<ILASTTree>)op3.Target];
					}
					else
					{
						bool flag4 = instr.Operand2 is IRJumpTable;
						if (flag4)
						{
							IRJumpTable op4 = (IRJumpTable)instr.Operand2;
							for (int j = 0; j < op4.Targets.Length; j++)
							{
								op4.Targets[j] = blockMap[(BasicBlock<ILASTTree>)op4.Targets[j]];
							}
						}
					}
				}
			});
		}

		// Token: 0x04000092 RID: 146
		private static readonly Dictionary<Code, ITranslationHandler> handlers = new Dictionary<Code, ITranslationHandler>();
	}
}
