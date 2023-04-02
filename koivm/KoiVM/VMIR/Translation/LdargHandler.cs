using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x02000068 RID: 104
	public class LdargHandler : ITranslationHandler
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000B4C4 File Offset: 0x000096C4
		public Code ILCode
		{
			get
			{
				return Code.Ldarg;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B4DC File Offset: 0x000096DC
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable param = tr.Context.ResolveParameter((Parameter)expr.Operand);
			IRVariable ret = tr.Context.AllocateVRegister(param.Type);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, ret, param));
			bool flag = param.RawType.ElementType == ElementType.I1 || param.RawType.ElementType == ElementType.I2;
			if (flag)
			{
				ret.RawType = param.RawType;
				IRVariable r = tr.Context.AllocateVRegister(param.Type);
				tr.Instructions.Add(new IRInstruction(IROpCode.SX, r, ret));
				ret = r;
			}
			return ret;
		}
	}
}
