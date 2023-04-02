using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200007B RID: 123
	public class SizeofHandler : ITranslationHandler
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000C014 File Offset: 0x0000A214
		public Code ILCode
		{
			get
			{
				return Code.Sizeof;
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000C02C File Offset: 0x0000A22C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			int typeId = (int)tr.Runtime.Descriptor.Data.GetId((ITypeDefOrRef)expr.Operand);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int ecallId = tr.VM.Runtime.VMCall.SIZEOF;
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
