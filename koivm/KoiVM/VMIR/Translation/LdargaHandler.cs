using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200006A RID: 106
	public class LdargaHandler : ITranslationHandler
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B60C File Offset: 0x0000980C
		public Code ILCode
		{
			get
			{
				return Code.Ldarga;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B624 File Offset: 0x00009824
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable param = tr.Context.ResolveParameter((Parameter)expr.Operand);
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.ByRef);
			tr.Instructions.Add(new IRInstruction(IROpCode.__LEA, ret, param));
			return ret;
		}
	}
}
