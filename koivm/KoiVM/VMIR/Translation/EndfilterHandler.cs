using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005C RID: 92
	public class EndfilterHandler : ITranslationHandler
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public Code ILCode
		{
			get
			{
				return Code.Endfilter;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000AB0C File Offset: 0x00008D0C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			tr.Instructions.Add(new IRInstruction(IROpCode.__EHRET, tr.Translate(expr.Arguments[0])));
			tr.Block.Flags |= BlockFlags.ExitEHReturn;
			return null;
		}
	}
}
