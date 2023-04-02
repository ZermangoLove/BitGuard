using System;
using KoiVM.AST.IL;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000E5 RID: 229
	public class EntryExitTransform : ITransform
	{
		// Token: 0x06000366 RID: 870 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILTransformer tr)
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00002DFF File Offset: 0x00000FFF
		public void Transform(ILTransformer tr)
		{
			tr.Instructions.VisitInstrs<ILTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000120AC File Offset: 0x000102AC
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
		{
			bool flag = instr.OpCode == ILOpCode.__ENTRY;
			if (flag)
			{
				instrs.RemoveAt(index);
				index--;
			}
			else
			{
				bool flag2 = instr.OpCode == ILOpCode.__EXIT;
				if (flag2)
				{
					instrs[index] = new ILInstruction(ILOpCode.RET, null, instr);
				}
			}
		}
	}
}
