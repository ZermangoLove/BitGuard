using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200007F RID: 127
	public class CltHandler : ITranslationHandler
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		public Code ILCode
		{
			get
			{
				return Code.Clt;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			tr.Instructions.Add(new IRInstruction(IROpCode.CMP)
			{
				Operand1 = tr.Translate(expr.Arguments[0]),
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.I4);
			IRVariable fl = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = fl,
				Operand2 = IRConstant.FromI4((1 << tr.Arch.Flags.OVERFLOW) | (1 << tr.Arch.Flags.SIGN))
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = fl
			});
			TranslationHelpers.EmitCompareEq(tr, ASTType.I4, ret, IRConstant.FromI4((1 << tr.Arch.Flags.OVERFLOW) | (1 << tr.Arch.Flags.SIGN)));
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__AND)
			{
				Operand1 = fl,
				Operand2 = fl
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = fl,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__OR)
			{
				Operand1 = ret,
				Operand2 = fl
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
			});
			return ret;
		}
	}
}
