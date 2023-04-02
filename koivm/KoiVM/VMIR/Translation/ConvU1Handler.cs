using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003A RID: 58
	public class ConvU1Handler : ITranslationHandler
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00007FD0 File Offset: 0x000061D0
		public Code ILCode
		{
			get
			{
				return Code.Conv_U1;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007FE8 File Offset: 0x000061E8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			retVar.RawType = tr.Context.Method.Module.CorLibTypes.Byte;
			switch (valueType)
			{
			case ASTType.I4:
			case ASTType.I8:
			case ASTType.Ptr:
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
				return retVar;
			case ASTType.R4:
			case ASTType.R8:
			{
				IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I8);
				tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmp, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, tmp));
				return retVar;
			}
			}
			throw new NotSupportedException();
		}
	}
}
