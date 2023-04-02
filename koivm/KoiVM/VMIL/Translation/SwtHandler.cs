using System;
using System.Diagnostics;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000CB RID: 203
	public class SwtHandler : ITranslationHandler
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0001123C File Offset: 0x0000F43C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.SWT;
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00011250 File Offset: 0x0000F450
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			tr.PushOperand(instr.Operand1);
			ILInstruction lastInstr = tr.Instructions[tr.Instructions.Count - 1];
			Debug.Assert(lastInstr.OpCode == ILOpCode.PUSHI_DWORD && lastInstr.Operand is ILJumpTable);
			ILInstruction switchInstr = new ILInstruction(ILOpCode.SWT)
			{
				Annotation = InstrAnnotation.JUMP
			};
			tr.Instructions.Add(switchInstr);
			ILJumpTable jmpTable = (ILJumpTable)lastInstr.Operand;
			jmpTable.Chunk.runtime = tr.Runtime;
			jmpTable.RelativeBase = switchInstr;
			tr.Runtime.AddChunk(jmpTable.Chunk);
		}
	}
}
