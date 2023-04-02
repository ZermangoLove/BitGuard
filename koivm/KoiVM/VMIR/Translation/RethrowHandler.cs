using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;
using KoiVM.CFG;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200005F RID: 95
	public class RethrowHandler : ITranslationHandler
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000AC58 File Offset: 0x00008E58
		public Code ILCode
		{
			get
			{
				return Code.Rethrow;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000AC70 File Offset: 0x00008E70
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 0);
			ScopeBlock[] parentScopes = tr.RootScope.SearchBlock(tr.Block);
			ScopeBlock catchScope = parentScopes[parentScopes.Length - 1];
			bool flag = catchScope.Type != ScopeType.Handler || catchScope.ExceptionHandler.HandlerType > ExceptionHandlerType.Catch;
			if (flag)
			{
				throw new InvalidProgramException();
			}
			IRVariable exVar = tr.Context.ResolveExceptionVar(catchScope.ExceptionHandler);
			Debug.Assert(exVar != null);
			int ecallId = tr.VM.Runtime.VMCall.THROW;
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, exVar));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL)
			{
				Operand1 = IRConstant.FromI4(ecallId),
				Operand2 = IRConstant.FromI4(1)
			});
			return null;
		}
	}
}
