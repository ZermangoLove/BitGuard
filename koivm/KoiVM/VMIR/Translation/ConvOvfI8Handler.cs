using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000048 RID: 72
	public class ConvOvfI8Handler : ITranslationHandler
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00008E3C File Offset: 0x0000703C
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I8;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008E54 File Offset: 0x00007054
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.I8;
			IIROperand iiroperand;
			if (flag)
			{
				iiroperand = value;
			}
			else
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I8);
				int ckovf = tr.VM.Runtime.VMCall.CKOVERFLOW;
				switch (valueType)
				{
				case ASTType.I4:
					tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, value));
					goto IL_13E;
				case ASTType.R4:
				case ASTType.R8:
				{
					IRVariable fl = tr.Context.AllocateVRegister(ASTType.I4);
					tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, retVar, value));
					tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
					{
						Operand1 = fl,
						Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
					});
					tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf), fl));
					goto IL_13E;
				}
				case ASTType.Ptr:
					tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
					goto IL_13E;
				}
				throw new NotSupportedException();
				IL_13E:
				iiroperand = retVar;
			}
			return iiroperand;
		}
	}
}
