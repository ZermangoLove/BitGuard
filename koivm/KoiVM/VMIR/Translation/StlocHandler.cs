using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x020000A0 RID: 160
	public class StlocHandler : ITranslationHandler
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000E428 File Offset: 0x0000C628
		public Code ILCode
		{
			get
			{
				return Code.Stloc;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000E440 File Offset: 0x0000C640
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = tr.Context.ResolveLocal((Local)expr.Operand),
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			return null;
		}
	}
}
