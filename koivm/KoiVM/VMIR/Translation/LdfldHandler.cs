using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000062 RID: 98
	public class LdfldHandler : ITranslationHandler
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000AFE8 File Offset: 0x000091E8
		public Code ILCode
		{
			get
			{
				return Code.Ldfld;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000AFFC File Offset: 0x000091FC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand obj = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int fieldId = (int)tr.VM.Data.GetId((IField)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.LDFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, obj));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
