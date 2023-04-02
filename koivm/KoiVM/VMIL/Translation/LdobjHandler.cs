using System;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000DA RID: 218
	public class LdobjHandler : ITranslationHandler
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0001151C File Offset: 0x0000F71C
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.__LDOBJ;
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00011530 File Offset: 0x0000F730
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand1);
			TypeSig rawType = ((PointerInfo)instr.Annotation).PointerType.ToTypeSig(true);
			tr.Instructions.Add(new ILInstruction(TranslationHelpers.GetLIND(instr.Operand2.Type, rawType)));
			tr.PopOperand(instr.Operand2);
		}
	}
}
