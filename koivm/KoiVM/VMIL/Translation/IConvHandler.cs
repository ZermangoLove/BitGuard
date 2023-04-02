using System;
using System.Diagnostics;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C6 RID: 198
	public class IConvHandler : ITranslationHandler
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00010FDC File Offset: 0x0000F1DC
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.ICONV;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			Debug.Assert(instr.Operand1.Type == ASTType.I8);
			switch (instr.Operand2.Type)
			{
			case ASTType.R4:
				tr.Instructions.Add(new ILInstruction(ILOpCode.FCONV_R32_R64));
				tr.Instructions.Add(new ILInstruction(ILOpCode.ICONV_R64));
				goto IL_A2;
			case ASTType.R8:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ICONV_R64));
				goto IL_A2;
			case ASTType.Ptr:
				tr.Instructions.Add(new ILInstruction(ILOpCode.ICONV_PTR));
				goto IL_A2;
			}
			throw new NotSupportedException();
			IL_A2:
			tr.PopOperand(instr.Operand1);
		}
	}
}
