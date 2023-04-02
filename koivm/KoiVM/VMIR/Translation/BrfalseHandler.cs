using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008A RID: 138
	public class BrfalseHandler : ITranslationHandler
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		public Code ILCode
		{
			get
			{
				return Code.Brfalse;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand val = tr.Translate(expr.Arguments[0]);
			TranslationHelpers.EmitCompareEq(tr, expr.Arguments[0].Type.Value, val, IRConstant.FromI4(0));
			IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = tmp,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.JNZ)
			{
				Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand),
				Operand2 = tmp
			});
			return null;
		}
	}
}
