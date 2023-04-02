using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003B RID: 59
	public class ConvI1Handler : ITranslationHandler
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000080D0 File Offset: 0x000062D0
		public Code ILCode
		{
			get
			{
				return Code.Conv_I1;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000080E4 File Offset: 0x000062E4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable t = tr.Context.AllocateVRegister(ASTType.I4);
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			t.RawType = tr.Context.Method.Module.CorLibTypes.SByte;
			switch (valueType)
			{
			case ASTType.I4:
			case ASTType.I8:
			case ASTType.Ptr:
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV, t, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, t));
				return retVar;
			case ASTType.R4:
			case ASTType.R8:
			{
				IRVariable tmp = tr.Context.AllocateVRegister(ASTType.I8);
				tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmp, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV, t, tmp));
				tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, t));
				return retVar;
			}
			}
			throw new NotSupportedException();
		}
	}
}
