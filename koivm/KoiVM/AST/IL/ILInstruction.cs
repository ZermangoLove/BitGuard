using System;
using System.Text;
using KoiVM.AST.IR;
using KoiVM.VMIL;

namespace KoiVM.AST.IL
{
	// Token: 0x02000143 RID: 323
	public class ILInstruction : ASTNode, IHasOffset
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x00003C41 File Offset: 0x00001E41
		public ILInstruction(ILOpCode opCode)
		{
			this.OpCode = opCode;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00003C53 File Offset: 0x00001E53
		public ILInstruction(ILOpCode opCode, IILOperand operand)
		{
			this.OpCode = opCode;
			this.Operand = operand;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00003C6D File Offset: 0x00001E6D
		public ILInstruction(ILOpCode opCode, IILOperand operand, object annotation)
		{
			this.OpCode = opCode;
			this.Operand = operand;
			this.Annotation = annotation;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00003C8F File Offset: 0x00001E8F
		public ILInstruction(ILOpCode opCode, IILOperand operand, ILInstruction origin)
		{
			this.OpCode = opCode;
			this.Operand = operand;
			this.Annotation = origin.Annotation;
			this.IR = origin.IR;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00003CC3 File Offset: 0x00001EC3
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00003CCB File Offset: 0x00001ECB
		public uint Offset { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00003CD4 File Offset: 0x00001ED4
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00003CDC File Offset: 0x00001EDC
		public IRInstruction IR { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00003CE5 File Offset: 0x00001EE5
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00003CED File Offset: 0x00001EED
		public ILOpCode OpCode { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00003CF6 File Offset: 0x00001EF6
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00003CFE File Offset: 0x00001EFE
		public IILOperand Operand { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00003D07 File Offset: 0x00001F07
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00003D0F File Offset: 0x00001F0F
		public object Annotation { get; set; }

		// Token: 0x06000588 RID: 1416 RVA: 0x0001CF4C File Offset: 0x0001B14C
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("{0}", this.OpCode.ToString().PadLeft(16));
			bool flag = this.Operand != null;
			if (flag)
			{
				ret.AppendFormat(" {0}", this.Operand);
			}
			bool flag2 = this.Annotation != null;
			if (flag2)
			{
				ret.AppendFormat("    ; {0}", this.Annotation);
			}
			return ret.ToString();
		}
	}
}
