using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200004E RID: 78
	public class ConvOvfU8Handler : ITranslationHandler
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000099A8 File Offset: 0x00007BA8
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_U8;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000099C0 File Offset: 0x00007BC0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I8);
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
				goto IL_1DA;
			default:
				goto IL_1DA;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(long.MaxValue)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
			return retVar;
			IL_1DA:
			throw new NotSupportedException();
		}
	}
}
