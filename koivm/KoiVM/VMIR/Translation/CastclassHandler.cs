using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000046 RID: 70
	public class CastclassHandler : ITranslationHandler
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00008B6C File Offset: 0x00006D6C
		public Code ILCode
		{
			get
			{
				return Code.Castclass;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008B80 File Offset: 0x00006D80
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			int typeId = (int)(tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand) | 2147483648U);
			int ecallId = tr.VM.Runtime.VMCall.CAST;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
