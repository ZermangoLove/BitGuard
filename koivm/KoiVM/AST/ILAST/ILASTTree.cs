using System;
using System.Collections.Generic;
using System.Text;

namespace KoiVM.AST.ILAST
{
	// Token: 0x0200014F RID: 335
	public class ILASTTree : List<IILASTStatement>
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00003E6C File Offset: 0x0000206C
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00003E74 File Offset: 0x00002074
		public ILASTVariable[] StackRemains { get; set; }

		// Token: 0x060005BF RID: 1471 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			foreach (IILASTStatement st in this)
			{
				ret.AppendLine(st.ToString());
			}
			ret.AppendLine();
			ret.Append("[");
			for (int i = 0; i < this.StackRemains.Length; i++)
			{
				bool flag = i != 0;
				if (flag)
				{
					ret.Append(", ");
				}
				ret.Append(this.StackRemains[i]);
			}
			ret.AppendLine("]");
			return ret.ToString();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001D5B8 File Offset: 0x0001B7B8
		public void TraverseTree<T>(Action<ILASTExpression, T> visitFunc, T state)
		{
			foreach (IILASTStatement st in this)
			{
				bool flag = st is ILASTExpression;
				if (flag)
				{
					this.TraverseTreeInternal<T>((ILASTExpression)st, visitFunc, state);
				}
				else
				{
					bool flag2 = st is ILASTAssignment;
					if (flag2)
					{
						this.TraverseTreeInternal<T>(((ILASTAssignment)st).Value, visitFunc, state);
					}
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001D644 File Offset: 0x0001B844
		private void TraverseTreeInternal<T>(ILASTExpression expr, Action<ILASTExpression, T> visitFunc, T state)
		{
			foreach (IILASTNode arg in expr.Arguments)
			{
				bool flag = arg is ILASTExpression;
				if (flag)
				{
					this.TraverseTreeInternal<T>((ILASTExpression)arg, visitFunc, state);
				}
			}
			visitFunc(expr, state);
		}
	}
}
