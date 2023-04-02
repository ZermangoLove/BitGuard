using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000099 RID: 153
	public class MulOvfUnHandler : ITranslationHandler
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000DD54 File Offset: 0x0000BF54
		public Code ILCode
		{
			get
			{
				return Code.Mul_Ovf_Un;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.MUL)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
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
