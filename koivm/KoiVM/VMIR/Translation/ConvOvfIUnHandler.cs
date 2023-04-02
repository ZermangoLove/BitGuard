using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005A RID: 90
	public class ConvOvfIUnHandler : ITranslationHandler
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000AA70 File Offset: 0x00008C70
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I_Un;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A968 File Offset: 0x00008B68
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
					tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
					{
						Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
					});
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
