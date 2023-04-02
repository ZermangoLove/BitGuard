using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000093 RID: 147
	public class AddOvfUnHandler : ITranslationHandler
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000D378 File Offset: 0x0000B578
		public Code ILCode
		{
			get
			{
				return Code.Add_Ovf_Un;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000D390 File Offset: 0x0000B590
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
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
