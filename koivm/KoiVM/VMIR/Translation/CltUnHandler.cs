using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000080 RID: 128
	public class CltUnHandler : ITranslationHandler
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C70C File Offset: 0x0000A90C
		public Code ILCode
		{
			get
			{
				return Code.Clt_Un;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000C724 File Offset: 0x0000A924
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			tr.Instructions.Add(new IRInstruction(IROpCode.CMP)
			{
				Operand1 = tr.Translate(expr.Arguments[0]),
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = ret,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.CARRY)
			});
			return ret;
		}
	}
}
