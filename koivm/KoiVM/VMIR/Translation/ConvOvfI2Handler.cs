using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200004C RID: 76
	public class ConvOvfI2Handler : ITranslationHandler
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000095B0 File Offset: 0x000077B0
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I2;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000095C8 File Offset: 0x000077C8
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
				goto IL_1EC;
			default:
				goto IL_1EC;
			}
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(-32768L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(32767L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, t, value));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, retVar, t));
			return retVar;
			IL_1EC:
			throw new NotSupportedException();
		}
	}
}
