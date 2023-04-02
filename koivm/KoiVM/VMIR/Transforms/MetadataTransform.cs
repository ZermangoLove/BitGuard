using System;
using dnlib.DotNet;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A9 RID: 169
	public class MetadataTransform : ITransform
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000027D3 File Offset: 0x000009D3
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000027EF File Offset: 0x000009EF
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			instr.Operand1 = this.TransformMD(instr.Operand1, tr);
			instr.Operand2 = this.TransformMD(instr.Operand2, tr);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000F40C File Offset: 0x0000D60C
		private IIROperand TransformMD(IIROperand operand, IRTransformer tr)
		{
			bool flag = operand is IRMetaTarget;
			if (flag)
			{
				IRMetaTarget target = (IRMetaTarget)operand;
				bool flag2 = !target.LateResolve;
				if (flag2)
				{
					bool flag3 = !(target.MetadataItem is IMemberRef);
					if (flag3)
					{
						throw new NotSupportedException();
					}
					return IRConstant.FromI4((int)tr.VM.Data.GetId((IMemberRef)target.MetadataItem));
				}
			}
			return operand;
		}
	}
}
