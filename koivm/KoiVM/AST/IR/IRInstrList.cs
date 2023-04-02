using System;
using System.Collections.Generic;

namespace KoiVM.AST.IR
{
	// Token: 0x02000139 RID: 313
	public class IRInstrList : List<IRInstruction>
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x0001CD28 File Offset: 0x0001AF28
		public override string ToString()
		{
			return string.Join<IRInstruction>(Environment.NewLine, this);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001CD48 File Offset: 0x0001AF48
		public void VisitInstrs<T>(VisitFunc<IRInstrList, IRInstruction, T> visitFunc, T arg)
		{
			for (int i = 0; i < base.Count; i++)
			{
				visitFunc(this, base[i], ref i, arg);
			}
		}
	}
}
