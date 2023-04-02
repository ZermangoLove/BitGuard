using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200007A RID: 122
	public class LocallocHandler : ITranslationHandler
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000BF68 File Offset: 0x0000A168
		public Code ILCode
		{
			get
			{
				return Code.Localloc;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000BF80 File Offset: 0x0000A180
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand size = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int ecallId = tr.VM.Runtime.VMCall.LOCALLOC;
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), size));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
