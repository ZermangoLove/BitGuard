using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000038 RID: 56
	public class ConvI4Handler : ITranslationHandler
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00007E04 File Offset: 0x00006004
		public Code ILCode
		{
			get
			{
				return Code.Conv_I4;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007E18 File Offset: 0x00006018
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
					IRVariable tmpVar = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmpVar, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, tmpVar));
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
