using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000044 RID: 68
	public class ConvRUnHandler : ITranslationHandler
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000895C File Offset: 0x00006B5C
		public Code ILCode
		{
			get
			{
				return Code.Conv_R_Un;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008970 File Offset: 0x00006B70
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.R8);
			ASTType asttype = valueType;
			ASTType asttype2 = asttype;
			if (asttype2 != ASTType.I4)
			{
				if (asttype2 != ASTType.I8)
				{
					throw new NotSupportedException();
				}
				tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
				{
					Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, value));
			}
			else
			{
				IRVariable tmpVar = tr.Context.AllocateVRegister(ASTType.I8);
				tr.Instructions.Add(new IRInstruction(IROpCode.SX, tmpVar, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
				{
					Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, retVar, tmpVar));
			}
			return retVar;
		}
	}
}
