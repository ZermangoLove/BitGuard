using System;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005B RID: 91
	public class LeaveHandler : ITranslationHandler
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000AA88 File Offset: 0x00008C88
		public Code ILCode
		{
			get
			{
				return Code.Leave;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.__LEAVE)
			{
				Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand)
			});
			tr.Block.Flags |= BlockFlags.ExitEHLeave;
			return null;
		}
	}
}
