using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000077 RID: 119
	public class LdobjHandler : ITranslationHandler
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000BD98 File Offset: 0x00009F98
		public Code ILCode
		{
			get
			{
				return Code.Ldobj;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000BDAC File Offset: 0x00009FAC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand addr = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.__LDOBJ, addr, retVar)
			{
				Annotation = new PointerInfo("LDOBJ", (ITypeDefOrRef)expr.Operand)
			});
			return retVar;
		}
	}
}
