using System;
using System.Collections.Generic;
using KoiVM.AST.IL;
using KoiVM.VM;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000E4 RID: 228
	public class FixMethodRefTransform : IPostTransform
	{
		// Token: 0x06000362 RID: 866 RVA: 0x00002DBA File Offset: 0x00000FBA
		public void Initialize(ILPostTransformer tr)
		{
			this.saveRegs = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method).UsedRegister;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00002DE3 File Offset: 0x00000FE3
		public void Transform(ILPostTransformer tr)
		{
			tr.Instructions.VisitInstrs<ILPostTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILPostTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012064 File Offset: 0x00010264
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILPostTransformer tr)
		{
			ILRelReference rel = instr.Operand as ILRelReference;
			bool flag = rel == null;
			if (!flag)
			{
				ILMethodTarget methodRef = rel.Target as ILMethodTarget;
				bool flag2 = methodRef == null;
				if (!flag2)
				{
					methodRef.Resolve(tr.Runtime);
				}
			}
		}

		// Token: 0x04000166 RID: 358
		private HashSet<VMRegisters> saveRegs;
	}
}
