using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008E RID: 142
	public class NewobjHandler : ITranslationHandler
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		public Code ILCode
		{
			get
			{
				return Code.Newobj;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			InstrCallInfo callInfo = new InstrCallInfo("NEWOBJ")
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
			IRVariable retVal = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.__NEWOBJ)
			{
				Operand1 = new IRMetaTarget(callInfo.Method),
				Operand2 = retVal,
				Annotation = callInfo
			});
			callInfo.ReturnValue = retVal;
			tr.Instructions.Add(new IRInstruction(IROpCode.__ENDCALL)
			{
				Annotation = callInfo
			});
			return retVal;
		}
	}
}
