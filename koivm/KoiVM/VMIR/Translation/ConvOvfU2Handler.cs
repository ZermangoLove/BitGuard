using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200004B RID: 75
	public class ConvOvfU2Handler : ITranslationHandler
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000093B8 File Offset: 0x000075B8
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_U2;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000093D0 File Offset: 0x000075D0
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			retVar.RawType = tr.Context.Method.Module.CorLibTypes.UInt16;
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
				goto IL_1C4;
			default:
				goto IL_1C4;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(65535L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
			return retVar;
			IL_1C4:
			throw new NotSupportedException();
		}
	}
}
