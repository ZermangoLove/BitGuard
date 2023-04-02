using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000079 RID: 121
	public class InitobjHandler : ITranslationHandler
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000BEB8 File Offset: 0x0000A0B8
		public Code ILCode
		{
			get
			{
				return Code.Initobj;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand addr = tr.Translate(expr.Arguments[0]);
			int typeId = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
			int ecallId = tr.VM.Runtime.VMCall.INITOBJ;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, addr));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			return null;
		}
	}
}
