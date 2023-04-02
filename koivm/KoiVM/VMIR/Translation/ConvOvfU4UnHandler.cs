using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000057 RID: 87
	public class ConvOvfU4UnHandler : ITranslationHandler
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000A6FC File Offset: 0x000088FC
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_U4_Un;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A714 File Offset: 0x00008914
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			int rangechk = tr.VM.Runtime.VMCall.RANGECHK;
			int ckovf = tr.VM.Runtime.VMCall.CKOVERFLOW;
			switch (valueType)
			{
			case ASTType.I4:
			case ASTType.I8:
			case ASTType.Ptr:
				break;
			case ASTType.R4:
			case ASTType.R8:
			{
				IRVariable tmpVar = tr.Context.AllocateVRegister(ASTType.I8);
				IRVariable fl = tr.Context.AllocateVRegister(ASTType.I4);
				tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
				{
					Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, tmpVar, value));
				tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
				{
					Operand1 = fl,
					Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
				});
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf), fl));
				value = tmpVar;
				break;
			}
			case ASTType.O:
				goto IL_1D3;
			default:
				goto IL_1D3;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8((long)((ulong)(-1)))));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
			return retVar;
			IL_1D3:
			throw new NotSupportedException();
		}
	}
}
