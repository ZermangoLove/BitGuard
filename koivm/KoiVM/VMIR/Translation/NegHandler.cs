using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009C RID: 156
	public class NegHandler : ITranslationHandler
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000E064 File Offset: 0x0000C264
		public Code ILCode
		{
			get
			{
				return Code.Neg;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000E078 File Offset: 0x0000C278
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			bool flag = expr.Type != null && (expr.Type.Value == ASTType.R4 || expr.Type.Value == ASTType.R8);
			if (flag)
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = ret,
					Operand2 = IRConstant.FromI4(0)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.SUB)
				{
					Operand1 = ret,
					Operand2 = tr.Translate(expr.Arguments[0])
				});
			}
			else
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = ret,
					Operand2 = tr.Translate(expr.Arguments[0])
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.__NOT)
				{
					Operand1 = ret
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
				{
					Operand1 = ret,
					Operand2 = IRConstant.FromI4(1)
				});
			}
			return ret;
		}
	}
}
