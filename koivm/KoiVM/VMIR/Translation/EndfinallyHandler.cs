using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005D RID: 93
	public class EndfinallyHandler : ITranslationHandler
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000AB68 File Offset: 0x00008D68
		public Code ILCode
		{
			get
			{
				return Code.Endfinally;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000AB80 File Offset: 0x00008D80
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.__EHRET));
			tr.Block.Flags |= BlockFlags.ExitEHReturn;
			return null;
		}
	}
}
