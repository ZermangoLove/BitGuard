using System;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000DB RID: 219
	public class StobjHandler : ITranslationHandler
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00011594 File Offset: 0x0000F794
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__STOBJ;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000115A8 File Offset: 0x0000F7A8
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			tr.PushOperand(instr.Operand1);
			TypeSig rawType = ((PointerInfo)instr.Annotation).PointerType.ToTypeSig(true);
			tr.Instructions.Add(new ILInstruction(TranslationHelpers.GetSIND(instr.Operand2.Type, rawType)));
		}
	}
}
