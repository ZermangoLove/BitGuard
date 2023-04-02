using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000042 RID: 66
	public class ConvR4Handler : ITranslationHandler
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00008760 File Offset: 0x00006960
		public Code ILCode
		{
			get
			{
				return Code.Conv_R4;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008774 File Offset: 0x00006974
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.R4;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.R4);
				switch (valueType)
				{
				case ASTType.I4:
				{
					IRVariable tmpVar = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.SX, tmpVar, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, tmpVar));
					goto IL_D5;
				}
				case ASTType.I8:
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, value));
					goto IL_D5;
				case ASTType.R8:
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, value));
					goto IL_D5;
				}
				throw new NotSupportedException();
				IL_D5:
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
