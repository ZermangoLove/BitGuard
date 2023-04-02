using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006D RID: 109
	public class BreakHandler : ITranslationHandler
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000B730 File Offset: 0x00009930
		public Code ILCode
		{
			get
			{
				return Code.Break;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000B744 File Offset: 0x00009944
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			int ecallId = tr.VM.Runtime.VMCall.BREAK;
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId)));
			return null;
		}
	}
}
