using System;
using System.Text;
using KoiVM.AST.ILAST;
using KoiVM.VMIR;

namespace KoiVM.AST.IR
{
	// Token: 0x0200013A RID: 314
	public class IRInstruction : ASTNode
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x00003A1D File Offset: 0x00001C1D
		public IRInstruction(IROpCode opCode)
		{
			this.OpCode = opCode;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00003A2F File Offset: 0x00001C2F
		public IRInstruction(IROpCode opCode, IIROperand op1)
		{
			this.OpCode = opCode;
			this.Operand1 = op1;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00003A49 File Offset: 0x00001C49
		public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2)
		{
			this.OpCode = opCode;
			this.Operand1 = op1;
			this.Operand2 = op2;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00003A6B File Offset: 0x00001C6B
		public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2, object annotation)
		{
			this.OpCode = opCode;
			this.Operand1 = op1;
			this.Operand2 = op2;
			this.Annotation = annotation;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00003A96 File Offset: 0x00001C96
		public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2, IRInstruction origin)
		{
			this.OpCode = opCode;
			this.Operand1 = op1;
			this.Operand2 = op2;
			this.Annotation = origin.Annotation;
			this.ILAST = origin.ILAST;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00003AD4 File Offset: 0x00001CD4
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00003ADC File Offset: 0x00001CDC
		public IROpCode OpCode { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00003AE5 File Offset: 0x00001CE5
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00003AED File Offset: 0x00001CED
		public IILASTStatement ILAST { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00003AF6 File Offset: 0x00001CF6
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00003AFE File Offset: 0x00001CFE
		public IIROperand Operand1 { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00003B07 File Offset: 0x00001D07
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00003B0F File Offset: 0x00001D0F
		public IIROperand Operand2 { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00003B18 File Offset: 0x00001D18
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00003B20 File Offset: 0x00001D20
		public object Annotation { get; set; }

		// Token: 0x06000556 RID: 1366 RVA: 0x0001CD80 File Offset: 0x0001AF80
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("{0}", this.OpCode.ToString().PadLeft(16));
			bool flag = this.Operand1 != null;
			if (flag)
			{
				ret.AppendFormat(" {0}", this.Operand1);
				bool flag2 = this.Operand2 != null;
				if (flag2)
				{
					ret.AppendFormat(", {0}", this.Operand2);
				}
			}
			bool flag3 = this.Annotation != null;
			if (flag3)
			{
				ret.AppendFormat("    ; {0}", this.Annotation);
			}
			return ret.ToString();
		}
	}
}
