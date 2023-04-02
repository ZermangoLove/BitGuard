using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000087 RID: 135
	public class LdtokenHandler : ITranslationHandler
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000C914 File Offset: 0x0000AB14
		public Code ILCode
		{
			get
			{
				return Code.Ldtoken;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000C92C File Offset: 0x0000AB2C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int refId = (int)tr.VM.Data.GetId((IMemberRef)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.TOKEN;
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(refId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
