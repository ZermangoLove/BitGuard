using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000070 RID: 112
	public class AndHandler : ITranslationHandler
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000B890 File Offset: 0x00009A90
		public Code ILCode
		{
			get
			{
				return Code.And;
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B8A4 File Offset: 0x00009AA4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IRVariable ret = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__AND)
			{
				Operand1 = ret,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			return ret;
		}
	}
}
