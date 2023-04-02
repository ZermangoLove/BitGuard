using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200008D RID: 141
	public class CallvirtHandler : ITranslationHandler
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		public Code ILCode
		{
			get
			{
				return Code.Callvirt;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			InstrCallInfo callInfo = new InstrCallInfo("CALLVIRT")
			{
				Method = (IMethod)expr.Operand
			};
			bool flag = expr.Prefixes != null && expr.Prefixes[0].OpCode == OpCodes.Constrained;
			if (flag)
			{
				callInfo.ConstrainType = (ITypeDefOrRef)expr.Prefixes[0].Operand;
			}
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
			bool flag2 = expr.Type != null;
			if (flag2)
			{
				retVal = tr.Context.AllocateVRegister(expr.Type.Value);
				tr.Instructions.Add(new IRInstruction(IROpCode.__CALLVIRT)
				{
					Operand1 = new IRMetaTarget(callInfo.Method),
					Operand2 = retVal,
					Annotation = callInfo
				});
			}
			else
			{
				tr.Instructions.Add(new IRInstruction(IROpCode.__CALLVIRT)
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
