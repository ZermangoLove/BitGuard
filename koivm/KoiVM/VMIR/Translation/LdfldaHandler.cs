using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000066 RID: 102
	public class LdfldaHandler : ITranslationHandler
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000B31C File Offset: 0x0000951C
		public Code ILCode
		{
			get
			{
				return Code.Ldflda;
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B330 File Offset: 0x00009530
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand obj = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int fieldId = (int)(tr.VM.Data.GetId((IField)expr.Operand) | 2147483648U);
			int ecallId = tr.VM.Runtime.VMCall.LDFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, obj));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
