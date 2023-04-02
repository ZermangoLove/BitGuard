using System;
using KoiVM.AST.IL;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000E3 RID: 227
	public class ReferenceOffsetTransform : ITransform
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILTransformer tr)
		{
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00002D9E File Offset: 0x00000F9E
		public void Transform(ILTransformer tr)
		{
			tr.Instructions.VisitInstrs<ILTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011FE0 File Offset: 0x000101E0
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
		{
			bool flag = instr.OpCode == ILOpCode.PUSHI_DWORD && instr.Operand is IHasOffset;
			if (flag)
			{
				ILInstruction relBase = new ILInstruction(ILOpCode.PUSHR_QWORD, ILRegister.IP, instr);
				instr.OpCode = ILOpCode.PUSHI_DWORD;
				instr.Operand = new ILRelReference((IHasOffset)instr.Operand, relBase);
				instrs.Replace(index, new ILInstruction[]
				{
					relBase,
					instr,
					new ILInstruction(ILOpCode.ADD_QWORD, null, instr)
				});
			}
		}
	}
}
