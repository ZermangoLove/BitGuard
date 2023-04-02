using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003E RID: 62
	public class ConvU4Handler : ITranslationHandler
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00008438 File Offset: 0x00006638
		public Code ILCode
		{
			get
			{
				return Code.Conv_U4;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007E18 File Offset: 0x00006018
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.I4;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
				switch (valueType)
				{
				case ASTType.I8:
				case ASTType.Ptr:
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
					goto IL_C2;
				case ASTType.R4:
				case ASTType.R8:
				{
					IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmp, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, tmp));
					goto IL_C2;
				}
				}
				throw new NotSupportedException();
				IL_C2:
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
