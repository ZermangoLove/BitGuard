using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200004A RID: 74
	public class ConvOvfI1Handler : ITranslationHandler
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000091A0 File Offset: 0x000073A0
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I1;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000091B8 File Offset: 0x000073B8
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable t = tr.Context.AllocateVRegister(ASTType.I4);
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			t.RawType = tr.Context.Method.Module.CorLibTypes.SByte;
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
				goto IL_1E6;
			default:
				goto IL_1E6;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(-128L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(127L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, t, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, t));
			return retVar;
			IL_1E6:
			throw new NotSupportedException();
		}
	}
}
