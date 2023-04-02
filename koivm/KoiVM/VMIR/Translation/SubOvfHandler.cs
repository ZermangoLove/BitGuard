using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000095 RID: 149
	public class SubOvfHandler : ITranslationHandler
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000D690 File Offset: 0x0000B890
		public Code ILCode
		{
			get
			{
				return Code.Sub_Ovf;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
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
					Operand1 = tmp,
					Operand2 = IRConstant.FromI4(1)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
				{
					Operand1 = ret,
					Operand2 = tmp
				});
			}
			int ecallId = tr.VM.Runtime.VMCall.CKOVERFLOW;
			IRVariable fl = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = fl,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), fl));
			return ret;
		}
	}
}
