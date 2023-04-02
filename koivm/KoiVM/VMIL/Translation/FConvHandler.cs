using System;
using System.Diagnostics;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VMIR;

namespace KoiVM.VMIL.Translation
{
	// Token: 0x020000C5 RID: 197
	public class FConvHandler : ITranslationHandler
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00010ED0 File Offset: 0x0000F0D0
		public IROpCode IRCode
		{
			get
			{
				return IROpCode.FCONV;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		public void Translate(IRInstruction instr, ILTranslator tr)
		{
			tr.PushOperand(instr.Operand2);
			ASTType type = instr.Operand2.Type;
			ASTType asttype = type;
			if (asttype != ASTType.R4)
			{
				if (asttype != ASTType.R8)
				{
					Debug.Assert(instr.Operand2.Type == ASTType.I8);
					ASTType type2 = instr.Operand1.Type;
					ASTType asttype2 = type2;
					if (asttype2 != ASTType.R4)
					{
						if (asttype2 != ASTType.R8)
						{
							throw new NotSupportedException();
						}
						tr.Instructions.Add(new ILInstruction(ILOpCode.FCONV_R64));
					}
					else
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.FCONV_R32));
					}
				}
				else
				{
					Debug.Assert(instr.Operand1.Type == ASTType.R4);
					tr.Instructions.Add(new ILInstruction(ILOpCode.FCONV_R64_R32));
				}
			}
			else
			{
				Debug.Assert(instr.Operand1.Type == ASTType.R8);
				tr.Instructions.Add(new ILInstruction(ILOpCode.FCONV_R32_R64));
			}
			tr.PopOperand(instr.Operand1);
		}
	}
}
