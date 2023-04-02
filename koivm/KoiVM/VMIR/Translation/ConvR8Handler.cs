using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000043 RID: 67
	public class ConvR8Handler : ITranslationHandler
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00008860 File Offset: 0x00006A60
		public Code ILCode
		{
			get
			{
				return Code.Conv_R8;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008874 File Offset: 0x00006A74
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.R8;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.R8);
				switch (valueType)
				{
				case ASTType.I4:
				{
					IRVariable tmpVar = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.SX, tmpVar, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, tmpVar));
					break;
				}
				case ASTType.I8:
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, value));
					break;
				case ASTType.R4:
					tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, value));
					break;
				default:
					throw new NotSupportedException();
				}
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
