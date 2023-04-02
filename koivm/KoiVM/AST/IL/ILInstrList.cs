using System;
using System.Collections.Generic;

namespace KoiVM.AST.IL
{
	// Token: 0x02000146 RID: 326
	public class ILInstrList : List<ILInstruction>
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0001D118 File Offset: 0x0001B318
		public override string ToString()
		{
			return string.Join<ILInstruction>(Environment.NewLine, this);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001D138 File Offset: 0x0001B338
		public void VisitInstrs<T>(VisitFunc<ILInstrList, ILInstruction, T> visitFunc, T arg)
		{
			for (int i = 0; i < base.Count; i++)
			{
				visitFunc(this, base[i], ref i, arg);
			}
		}
	}
}
