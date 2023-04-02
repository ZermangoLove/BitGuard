using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000064 RID: 100
	public class LdsfldHandler : ITranslationHandler
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000B194 File Offset: 0x00009394
		public Code ILCode
		{
			get
			{
				return Code.Ldsfld;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B1A8 File Offset: 0x000093A8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int fieldId = (int)tr.VM.Data.GetId((IField)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.LDFLD;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.Null()));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(fieldId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
