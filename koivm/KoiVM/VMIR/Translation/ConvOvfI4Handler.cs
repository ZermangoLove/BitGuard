using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000047 RID: 71
	public class ConvOvfI4Handler : ITranslationHandler
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00008C50 File Offset: 0x00006E50
		public Code ILCode
		{
			get
			{
				return Code.Conv_Ovf_I4;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00008C68 File Offset: 0x00006E68
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			Debug.Assert(expr.Arguments.Length == 1);
			IIROperand value = tr.Translate(expr.Arguments[0]);
			ASTType valueType = value.Type;
			bool flag = valueType == ASTType.I4;
			if (!flag)
			{
				IRVariable retVar = tr.Context.AllocateVRegister(ASTType.I4);
				int rangechk = tr.VM.Runtime.VMCall.RANGECHK;
				int ckovf = tr.VM.Runtime.VMCall.CKOVERFLOW;
				switch (valueType)
				{
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
					goto IL_1B7;
				default:
					goto IL_1B7;
				}
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(-2147483648L)));
				tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(2147483647L)));
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rangechk), value));
				tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ckovf)));
				tr.Instructions.Add(new IRInstruction(IROpCode.MOV, retVar, value));
				return retVar;
				IL_1B7:
				throw new NotSupportedException();
			}
			return value;
		}
	}
}
