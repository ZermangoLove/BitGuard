using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000096 RID: 150
	public class SubOvfUnHandler : ITranslationHandler
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		public Code ILCode
		{
			get
			{
				return Code.Sub_Ovf_Un;
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D910 File Offset: 0x0000BB10
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
			int ecallId = tr.VM.Runtime.VMCall.CKOVERFLOW;
			IRVariable fl = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = fl,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.CARRY)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), fl));
			return ret;
		}
	}
}
