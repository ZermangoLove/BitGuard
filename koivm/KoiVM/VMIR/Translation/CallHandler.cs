using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008C RID: 140
	public class CallHandler : ITranslationHandler
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		public Code ILCode
		{
			get
			{
				return Code.Call;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000CC70 File Offset: 0x0000AE70
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			InstrCallInfo callInfo = new InstrCallInfo("CALL")
			{
				Method = (IMethod)expr.Operand
			};
			tr.Instructions.Add(new IRInstruction(IROpCode.__BEGINCALL)
			{
				Annotation = callInfo
			});
			IIROperand[] args = new IIROperand[expr.Arguments.Length];
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = tr.Translate(expr.Arguments[i]);
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH)
				{
					Operand1 = args[i],
					Annotation = callInfo
				});
			}
			callInfo.Arguments = args;
			IIROperand retVal = null;
			bool flag = expr.Type != null;
			if (flag)
			{
				retVal = tr.Context.AllocateVRegister(expr.Type.Value);
				tr.Instructions.Add(new IRInstruction(IROpCode.__CALL)
				{
					Operand1 = new IRMetaTarget(callInfo.Method),
					Operand2 = retVal,
					Annotation = callInfo
				});
			}
			else
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.__CALL)
				{
					Operand1 = new IRMetaTarget(callInfo.Method),
					Annotation = callInfo
				});
			}
			callInfo.ReturnValue = retVal;
			tr.Instructions.Add(new IRInstruction(IROpCode.__ENDCALL)
			{
				Annotation = callInfo
			});
			return retVal;
		}
	}
}
