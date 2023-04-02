using System;
using KoiVM.AST.IL;
using KoiVM.VM;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000E6 RID: 230
	public class SaveInfoTransform : ITransform
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00012100 File Offset: 0x00010300
		public void Initialize(ILTransformer tr)
		{
			this.methodInfo = tr.VM.Data.LookupInfo(tr.Method);
			this.methodInfo.RootScope = tr.RootScope;
			tr.VM.Data.SetInfo(tr.Method, this.methodInfo);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00002E1B File Offset: 0x0000101B
		public void Transform(ILTransformer tr)
		{
			tr.Instructions.VisitInstrs<ILTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00012158 File Offset: 0x00010358
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
		{
			bool flag = instr.Operand is ILRegister;
			if (flag)
			{
				VMRegisters reg = ((ILRegister)instr.Operand).Register;
				bool flag2 = reg.IsGPR();
				if (flag2)
				{
					this.methodInfo.UsedRegister.Add(reg);
				}
			}
		}

		// Token: 0x04000167 RID: 359
		private VMMethodInfo methodInfo;
	}
}
