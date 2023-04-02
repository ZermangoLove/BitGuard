using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009D RID: 157
	public class RemHandler : ITranslationHandler
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		public Code ILCode
		{
			get
			{
				return Code.Rem;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.REM)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
