using System;
using System.Diagnostics;
using KoiVM.AST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR
{
	// Token: 0x02000029 RID: 41
	public static class TranslationHelpers
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00006E08 File Offset: 0x00005008
		public static void EmitCompareEq(IRTranslator tr, ASTType type, IIROperand a, IIROperand b)
		{
			bool flag = type == ASTType.O || type == ASTType.ByRef || type == ASTType.R4 || type == ASTType.R8;
			if (flag)
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.CMP, a, b));
			}
			else
			{
				Debug.Assert(type == ASTType.I4 || type == ASTType.I8 || type == ASTType.Ptr);
				tr.Instructions.Add(new IRInstruction(IROpCode.CMP, a, b));
			}
		}
	}
}
