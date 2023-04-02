using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000041 RID: 65
	public class ConvIHandler : ITranslationHandler
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00008678 File Offset: 0x00006878
		public Code ILCode
		{
			get
			{
				return Code.Conv_I;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008690 File Offset: 0x00006890
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
