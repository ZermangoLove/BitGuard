using System;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x020000A1 RID: 161
	public class LdlocaHandler : ITranslationHandler
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		public Code ILCode
		{
			get
			{
				return Code.Ldloca;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable local = tr.Context.ResolveLocal((Local)expr.Operand);
			IRVariable ret = tr.Context.AllocateVRegister(ASTType.ByRef);
			tr.Instructions.Add(new IRInstruction(IROpCode.__LEA, ret, local));
			return ret;
		}
	}
}
