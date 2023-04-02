using System;
using KoiVM.VM;

namespace KoiVM.AST.IR
{
	// Token: 0x02000136 RID: 310
	public class IRRegister : IIROperand
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x000039A4 File Offset: 0x00001BA4
		public IRRegister(VMRegisters reg)
		{
			this.Register = reg;
			this.Type = ASTType.Ptr;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000039BE File Offset: 0x00001BBE
		public IRRegister(VMRegisters reg, ASTType type)
		{
			this.Register = reg;
			this.Type = type;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x000039D8 File Offset: 0x00001BD8
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x000039E0 File Offset: 0x00001BE0
		public VMRegisters Register { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x000039E9 File Offset: 0x00001BE9
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x000039F1 File Offset: 0x00001BF1
		public IRVariable SourceVariable { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000039FA File Offset: 0x00001BFA
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x00003A02 File Offset: 0x00001C02
		public ASTType Type { get; set; }

		// Token: 0x06000539 RID: 1337 RVA: 0x0001CB44 File Offset: 0x0001AD44
		public override string ToString()
		{
			return this.Register.ToString();
		}

		// Token: 0x0400023C RID: 572
		public static readonly IRRegister BP = new IRRegister(VMRegisters.BP, ASTType.I4);

		// Token: 0x0400023D RID: 573
		public static readonly IRRegister SP = new IRRegister(VMRegisters.SP, ASTType.I4);

		// Token: 0x0400023E RID: 574
		public static readonly IRRegister IP = new IRRegister(VMRegisters.IP);

		// Token: 0x0400023F RID: 575
		public static readonly IRRegister FL = new IRRegister(VMRegisters.FL, ASTType.I4);

		// Token: 0x04000240 RID: 576
		public static readonly IRRegister K1 = new IRRegister(VMRegisters.K1, ASTType.I4);

		// Token: 0x04000241 RID: 577
		public static readonly IRRegister K2 = new IRRegister(VMRegisters.K2, ASTType.I4);

		// Token: 0x04000242 RID: 578
		public static readonly IRRegister M1 = new IRRegister(VMRegisters.M1, ASTType.I4);

		// Token: 0x04000243 RID: 579
		public static readonly IRRegister M2 = new IRRegister(VMRegisters.M2, ASTType.I4);
	}
}
