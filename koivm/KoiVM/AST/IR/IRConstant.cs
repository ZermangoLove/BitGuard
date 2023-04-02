using System;

namespace KoiVM.AST.IR
{
	// Token: 0x02000138 RID: 312
	public class IRConstant : ASTConstant, IIROperand
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001CBE0 File Offset: 0x0001ADE0
		ASTType IIROperand.Type
		{
			get
			{
				return base.Type.Value;
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001CC00 File Offset: 0x0001AE00
		public static IRConstant FromI4(int value)
		{
			return new IRConstant
			{
				Value = value,
				Type = new ASTType?(ASTType.I4)
			};
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001CC34 File Offset: 0x0001AE34
		public static IRConstant FromI8(long value)
		{
			return new IRConstant
			{
				Value = value,
				Type = new ASTType?(ASTType.I8)
			};
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001CC68 File Offset: 0x0001AE68
		public static IRConstant FromR4(float value)
		{
			return new IRConstant
			{
				Value = value,
				Type = new ASTType?(ASTType.R4)
			};
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001CC9C File Offset: 0x0001AE9C
		public static IRConstant FromR8(double value)
		{
			return new IRConstant
			{
				Value = value,
				Type = new ASTType?(ASTType.R8)
			};
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001CCD0 File Offset: 0x0001AED0
		public static IRConstant FromString(string value)
		{
			return new IRConstant
			{
				Value = value,
				Type = new ASTType?(ASTType.O)
			};
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001CCFC File Offset: 0x0001AEFC
		public static IRConstant Null()
		{
			return new IRConstant
			{
				Value = null,
				Type = new ASTType?(ASTType.O)
			};
		}
	}
}
