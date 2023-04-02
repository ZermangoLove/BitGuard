using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000098 RID: 152
	public class MulOvfHandler : ITranslationHandler
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public Code ILCode
		{
			get
			{
				return Code.Mul_Ovf;
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
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
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), fl));
			return ret;
		}
	}
}
