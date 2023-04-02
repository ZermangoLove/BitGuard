using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200003D RID: 61
	public class ConvI2Handler : ITranslationHandler
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00008304 File Offset: 0x00006504
		public Code ILCode
		{
			get
			{
				return Code.Conv_I2;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008318 File Offset: 0x00006518
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable t = tr.Context.AllocateVRegister(ASTType.I4);
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			t.RawType = tr.Context.Method.Module.CorLibTypes.Int16;
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
