using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003C RID: 60
	public class ConvU2Handler : ITranslationHandler
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00008204 File Offset: 0x00006404
		public Code ILCode
		{
			get
			{
				return Code.Conv_U2;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000821C File Offset: 0x0000641C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			retVar.RawType = tr.Context.Method.Module.CorLibTypes.UInt16;
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
