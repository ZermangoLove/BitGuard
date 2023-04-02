using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000050 RID: 80
	public class ConvOvfIHandler : ITranslationHandler
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00009BCC File Offset: 0x00007DCC
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008690 File Offset: 0x00006890
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.Ptr || valueType == ASTType.I4;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.Ptr);
				ASTType asttype = valueType;
				ASTType asttype2 = asttype;
				if (asttype2 != ASTType.I8)
				{
					if (asttype2 - ASTType.R4 > 1)
					{
						throw new NotSupportedException();
					}
					IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmp, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, tmp));
				}
				else
				{
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
				}
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
