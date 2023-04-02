using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005E RID: 94
	public class ThrowHandler : ITranslationHandler
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000ABBC File Offset: 0x00008DBC
		public Code ILCode
		{
			get
			{
				return Code.Throw;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			int ecallId = tr.VM.Runtime.VMCall.THROW;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, tr.Translate(expr.Arguments[0])));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL)
			{
				Operand1 = IRConstant.FromI4(ecallId),
				Operand2 = IRConstant.FromI4(0)
			});
			return null;
		}
	}
}
