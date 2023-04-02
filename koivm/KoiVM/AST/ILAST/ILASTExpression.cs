using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;
using KoiVM.CFG;

namespace KoiVM.AST.ILAST
{
	// Token: 0x0200014C RID: 332
	public class ILASTExpression : ASTExpression, IILASTNode, IILASTStatement
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00003DD2 File Offset: 0x00001FD2
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00003DDA File Offset: 0x00001FDA
		public Code ILCode { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00003DE3 File Offset: 0x00001FE3
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00003DEB File Offset: 0x00001FEB
		public Instruction CILInstr { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00003DF4 File Offset: 0x00001FF4
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00003DFC File Offset: 0x00001FFC
		public object Operand { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00003E05 File Offset: 0x00002005
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00003E0D File Offset: 0x0000200D
		public IILASTNode[] Arguments { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00003E16 File Offset: 0x00002016
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x00003E1E File Offset: 0x0000201E
		public Instruction[] Prefixes { get; set; }

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001D288 File Offset: 0x0001B488
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("{0}{1}(", this.ILCode.ToOpCode().Name, (base.Type == null) ? "" : (":" + base.Type.Value.ToString()));
			bool flag = this.Operand != null;
			if (flag)
			{
				bool flag2 = this.Operand is string;
				if (flag2)
				{
					ASTConstant.EscapeString(ret, (string)this.Operand, true);
				}
				else
				{
					bool flag3 = this.Operand is IBasicBlock;
					if (flag3)
					{
						ret.AppendFormat("Block_{0:x2}", ((IBasicBlock)this.Operand).Id);
					}
					else
					{
						bool flag4 = this.Operand is IBasicBlock[];
						if (flag4)
						{
							IEnumerable<string> targets = ((IBasicBlock[])this.Operand).Select((IBasicBlock block) => string.Format("Block_{0:x2}", block.Id));
							ret.AppendFormat("[{0}]", string.Join(", ", targets));
						}
						else
						{
							ret.Append(this.Operand);
						}
					}
				}
				bool flag5 = this.Arguments.Length != 0;
				if (flag5)
				{
					ret.Append(";");
				}
			}
			for (int i = 0; i < this.Arguments.Length; i++)
			{
				bool flag6 = i != 0;
				if (flag6)
				{
					ret.Append(",");
				}
				ret.Append(this.Arguments[i]);
			}
			ret.Append(")");
			return ret.ToString();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001D454 File Offset: 0x0001B654
		public ILASTExpression Clone()
		{
			return (ILASTExpression)base.MemberwiseClone();
		}
	}
}
