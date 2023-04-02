using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000094 RID: 148
	public class SubHandler : ITranslationHandler
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		public Code ILCode
		{
			get
			{
				return Code.Sub;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			bool flag = expr.Type != null && (expr.Type.Value == ASTType.R4 || expr.Type.Value == ASTType.R8);
			if (flag)
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = ret,
					Operand2 = tr.Translate(expr.Arguments[0])
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.SUB)
				{
					Operand1 = ret,
					Operand2 = tr.Translate(expr.Arguments[1])
				});
			}
			else
			{
				IRVariable tmp = tr.Context.AllocateVRegister(expr.Type.Value);
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = ret,
					Operand2 = tr.Translate(expr.Arguments[0])
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = tmp,
					Operand2 = tr.Translate(expr.Arguments[1])
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.__NOT)
				{
					Operand1 = tmp
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
				{
					Operand1 = ret,
					Operand2 = tmp
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
