using System;
using System.Collections.Generic;
using KoiVM.VM;

namespace KoiVM.AST.IL
{
	// Token: 0x02000144 RID: 324
	public class ILRegister : IILOperand
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x00003D18 File Offset: 0x00001F18
		private ILRegister(VMRegisters reg)
		{
			this.Register = reg;
			ILRegister.regMap.Add(reg, this);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00003D37 File Offset: 0x00001F37
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00003D3F File Offset: 0x00001F3F
		public VMRegisters Register { get; set; }

		// Token: 0x0600058C RID: 1420 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
		public override string ToString()
		{
			return this.Register.ToString();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001CFFC File Offset: 0x0001B1FC
		public static ILRegister LookupRegister(VMRegisters reg)
		{
			return ILRegister.regMap[reg];
		}

		// Token: 0x0400025E RID: 606
		private static readonly Dictionary<VMRegisters, ILRegister> regMap = new Dictionary<VMRegisters, ILRegister>();

		// Token: 0x04000260 RID: 608
		public static readonly ILRegister R0 = new ILRegister(VMRegisters.R0);

		// Token: 0x04000261 RID: 609
		public static readonly ILRegister R1 = new ILRegister(VMRegisters.R1);

		// Token: 0x04000262 RID: 610
		public static readonly ILRegister R2 = new ILRegister(VMRegisters.R2);

		// Token: 0x04000263 RID: 611
		public static readonly ILRegister R3 = new ILRegister(VMRegisters.R3);

		// Token: 0x04000264 RID: 612
		public static readonly ILRegister R4 = new ILRegister(VMRegisters.R4);

		// Token: 0x04000265 RID: 613
		public static readonly ILRegister R5 = new ILRegister(VMRegisters.R5);

		// Token: 0x04000266 RID: 614
		public static readonly ILRegister R6 = new ILRegister(VMRegisters.R6);

		// Token: 0x04000267 RID: 615
		public static readonly ILRegister R7 = new ILRegister(VMRegisters.R7);

		// Token: 0x04000268 RID: 616
		public static readonly ILRegister BP = new ILRegister(VMRegisters.BP);

		// Token: 0x04000269 RID: 617
		public static readonly ILRegister SP = new ILRegister(VMRegisters.SP);

		// Token: 0x0400026A RID: 618
		public static readonly ILRegister IP = new ILRegister(VMRegisters.IP);

		// Token: 0x0400026B RID: 619
		public static readonly ILRegister FL = new ILRegister(VMRegisters.FL);

		// Token: 0x0400026C RID: 620
		public static readonly ILRegister K1 = new ILRegister(VMRegisters.K1);

		// Token: 0x0400026D RID: 621
		public static readonly ILRegister K2 = new ILRegister(VMRegisters.K2);

		// Token: 0x0400026E RID: 622
		public static readonly ILRegister M1 = new ILRegister(VMRegisters.M1);

		// Token: 0x0400026F RID: 623
		public static readonly ILRegister M2 = new ILRegister(VMRegisters.M2);
	}
}
