using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000039 RID: 57
	public class ConvI8Handler : ITranslationHandler
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007EF0 File Offset: 0x000060F0
		public Code ILCode
		{
			get
			{
				return Code.Conv_I8;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007F04 File Offset: 0x00006104
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.I8;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I8);
				switch (valueType)
				{
				case ASTType.I4:
					tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, value));
					goto IL_B7;
				case ASTType.R4:
				case ASTType.R8:
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, retVar, value));
					goto IL_B7;
				case ASTType.Ptr:
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
					goto IL_B7;
				}
				throw new NotSupportedException();
				IL_B7:
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
