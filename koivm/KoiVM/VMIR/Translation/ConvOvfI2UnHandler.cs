using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000056 RID: 86
	public class ConvOvfI2UnHandler : ITranslationHandler
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000A4AC File Offset: 0x000086AC
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I2_Un;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A4C4 File Offset: 0x000086C4
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			IRVariable t = tr.Context.AllocateVRegister(ASTType.I4);
			IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
			t.RawType = tr.Context.Method.Module.CorLibTypes.Int16;
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
				goto IL_21C;
			default:
				goto IL_21C;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(32767L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, t, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, t));
			return retVar;
			IL_21C:
			throw new NotSupportedException();
		}
	}
}
