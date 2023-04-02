using System;
using KoiVM.AST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A4 RID: 164
	public class MarkReturnRegTransform : ITransform
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00002771 File Offset: 0x00000971
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			InstrCallInfo callInfo = instr.Annotation as InstrCallInfo;
			bool flag = callInfo == null || callInfo.ReturnValue == null;
			if (!flag)
			{
				bool flag2 = instr.Operand1 is IRRegister && ((IRRegister)instr.Operand1).SourceVariable == callInfo.ReturnValue;
				if (flag2)
				{
					callInfo.ReturnRegister = (IRRegister)instr.Operand1;
				}
				else
				{
					bool flag3 = instr.Operand1 is IRPointer && ((IRPointer)instr.Operand1).SourceVariable == callInfo.ReturnValue;
					if (flag3)
					{
						callInfo.ReturnSlot = (IRPointer)instr.Operand1;
					}
				}
			}
		}
	}
}
