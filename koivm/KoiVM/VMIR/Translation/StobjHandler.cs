using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000078 RID: 120
	public class StobjHandler : ITranslationHandler
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000BE2C File Offset: 0x0000A02C
		public Code ILCode
		{
			get
			{
				return Code.Stobj;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000BE44 File Offset: 0x0000A044
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 2);
			IIROperand addr = tr.Translate(expr.Arguments[0]);
			IIROperand value = tr.Translate(expr.Arguments[1]);
			tr.Instructions.Add(new IRInstruction(IROpCode.__STOBJ, addr, value)
			{
				Annotation = new PointerInfo("STOBJ", (ITypeDefOrRef)expr.Operand)
			});
			return null;
		}
	}
}
