using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000061 RID: 97
	public class LdvirtftnHandler : ITranslationHandler
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000AF04 File Offset: 0x00009104
		public Code ILCode
		{
			get
			{
				return Code.Ldvirtftn;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000AF1C File Offset: 0x0000911C
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IRVariable retVar = tr.Context.AllocateVRegister(expr.Type.Value);
			IIROperand obj = tr.Translate(expr.Arguments[0]);
			IMethod method = (IMethod)expr.Operand;
			int methodId = (int)tr.VM.Data.GetId(method);
			int ecallId = tr.VM.Runtime.VMCall.LDFTN;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, obj));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(methodId)));
			tr.Instructions.Add(new IRInstruction(IROpCode.POP, retVar));
			return retVar;
		}
	}
}
