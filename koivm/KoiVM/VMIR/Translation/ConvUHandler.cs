using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000040 RID: 64
	public class ConvUHandler : ITranslationHandler
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00008558 File Offset: 0x00006758
		public Code ILCode
		{
			get
			{
				return Code.Conv_U;
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008570 File Offset: 0x00006770
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
