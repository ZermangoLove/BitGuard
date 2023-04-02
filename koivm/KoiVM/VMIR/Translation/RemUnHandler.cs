using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009E RID: 158
	public class RemUnHandler : ITranslationHandler
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000E278 File Offset: 0x0000C478
		public Code ILCode
		{
			get
			{
				return Code.Rem_Un;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000E28C File Offset: 0x0000C48C
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
			tr.Instructions.Add(new IRInstruction(IROpCode.REM)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
