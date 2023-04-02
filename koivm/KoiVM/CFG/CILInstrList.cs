using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace KoiVM.CFG
{
	// Token: 0x02000121 RID: 289
	public class CILInstrList : List<Instruction>
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
		public override string ToString()
		{
			return string.Join<Instruction>(Environment.NewLine, this);
		}
	}
}
