using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003F RID: 63
	public class ConvU8Handler : ITranslationHandler
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000844C File Offset: 0x0000664C
		public Code ILCode
		{
			get
			{
				return Code.Conv_U8;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008460 File Offset: 0x00006660
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
				case ASTType.Ptr:
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
					goto IL_E3;
				case ASTType.R4:
				case ASTType.R8:
				{
					IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I8);
					tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
					{
						Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
					});
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmp, value));
					goto IL_E3;
				}
				}
				throw new NotSupportedException();
				IL_E3:
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
