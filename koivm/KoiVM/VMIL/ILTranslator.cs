using System;
using System.Collections.Generic;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VM;
using KoiVM.VMIR;

namespace KoiVM.VMIL
{
	// Token: 0x020000B4 RID: 180
	public class ILTranslator
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		static ILTranslator()
		{
			foreach (Type type in typeof(ILTranslator).Assembly.GetExportedTypes())
			{
				bool flag = typeof(ITranslationHandler).IsAssignableFrom(type) && !type.IsAbstract;
				if (flag)
				{
					ITranslationHandler handler = (ITranslationHandler)Activator.CreateInstance(type);
					ILTranslator.handlers.Add(handler.IRCode, handler);
				}
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000029B3 File Offset: 0x00000BB3
		public ILTranslator(VMRuntime runtime)
		{
			this.Runtime = runtime;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002BA RID: 698 RVA: 0x000029C5 File Offset: 0x00000BC5
		// (set) Token: 0x060002BB RID: 699 RVA: 0x000029CD File Offset: 0x00000BCD
		public VMRuntime Runtime { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		public VMDescriptor VM
		{
			get
			{
				return this.Runtime.Descriptor;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002BD RID: 701 RVA: 0x000029D6 File Offset: 0x00000BD6
		// (set) Token: 0x060002BE RID: 702 RVA: 0x000029DE File Offset: 0x00000BDE
		internal ILInstrList Instructions { get; private set; }

		// Token: 0x060002BF RID: 703 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		public ILInstrList Translate(IRInstrList instrs)
		{
			this.Instructions = new ILInstrList();
			int i = 0;
			foreach (IRInstruction instr in instrs)
			{
				ITranslationHandler handler;
				bool flag = !ILTranslator.handlers.TryGetValue(instr.OpCode, out handler);
				if (flag)
				{
					throw new NotSupportedException(instr.OpCode.ToString());
				}
				try
				{
					handler.Translate(instr, this);
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("Failed to translate ir {0}.", instr.ILAST), ex);
				}
				while (i < this.Instructions.Count)
				{
					this.Instructions[i].IR = instr;
					i++;
				}
			}
			ILInstrList ret = this.Instructions;
			this.Instructions = null;
			return ret;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000FD80 File Offset: 0x0000DF80
		public void Translate(ScopeBlock rootScope)
		{
			Dictionary<BasicBlock<IRInstrList>, BasicBlock<ILInstrList>> blockMap = rootScope.UpdateBasicBlocks<IRInstrList, ILInstrList>((BasicBlock<IRInstrList> block) => this.Translate(block.Content), (int id, ILInstrList content) => new ILBlock(id, content));
			rootScope.ProcessBasicBlocks<ILInstrList>(delegate(BasicBlock<ILInstrList> block)
			{
				foreach (ILInstruction instr in block.Content)
				{
					bool flag = instr.Operand is ILBlockTarget;
					if (flag)
					{
						ILBlockTarget op = (ILBlockTarget)instr.Operand;
						op.Target = blockMap[(BasicBlock<IRInstrList>)op.Target];
					}
					else
					{
						bool flag2 = instr.Operand is ILJumpTable;
						if (flag2)
						{
							ILJumpTable op2 = (ILJumpTable)instr.Operand;
							for (int i = 0; i < op2.Targets.Length; i++)
							{
								op2.Targets[i] = blockMap[(BasicBlock<IRInstrList>)op2.Targets[i]];
							}
						}
					}
				}
			});
		}

		// Token: 0x04000144 RID: 324
		private static readonly Dictionary<IROpCode, ITranslationHandler> handlers = new Dictionary<IROpCode, ITranslationHandler>();
	}
}
