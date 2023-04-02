using System;
using System.Text;

namespace KoiVM.AST.ILAST
{
	// Token: 0x0200014E RID: 334
	public class ILASTPhi : ASTNode, IILASTStatement
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00003E4A File Offset: 0x0000204A
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00003E52 File Offset: 0x00002052
		public ILASTVariable Variable { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00003E5B File Offset: 0x0000205B
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x00003E63 File Offset: 0x00002063
		public ILASTVariable[] SourceVariables { get; set; }

		// Token: 0x060005BB RID: 1467 RVA: 0x0001D474 File Offset: 0x0001B674
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("{0} = [", this.Variable);
			for (int i = 0; i < this.SourceVariables.Length; i++)
			{
				bool flag = i != 0;
				if (flag)
				{
					ret.Append(", ");
				}
				ret.Append(this.SourceVariables[i]);
			}
			ret.Append("]");
			return ret.ToString();
		}
	}
}
