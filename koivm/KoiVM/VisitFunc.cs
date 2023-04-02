using System;

namespace KoiVM
{
	// Token: 0x0200000C RID: 12
	// (Invoke) Token: 0x0600005A RID: 90
	public delegate void VisitFunc<TList, TInstr, TState>(TList list, TInstr instr, ref int index, TState state);
}
