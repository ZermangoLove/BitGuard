using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Translation
{
	// Token: 0x0200009F RID: 159
	public class LdlocHandler : ITranslationHandler
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000E360 File Offset: 0x0000C560
		public Code ILCode
		{
			get
			{
				return Code.Ldloc;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000E378 File Offset: 0x0000C578
		public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
		{
			IRVariable local = tr.Context.ResolveLocal((Local)expr.Operand);
			IRVariable ret = tr.Context.AllocateVRegister(local.Type);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, ret, local));
			bool flag = local.RawType.ElementType == ElementType.I1 || local.RawType.ElementType == ElementType.I2;
			if (flag)
			{
				ret.RawType = local.RawType;
				IRVariable r = tr.Context.AllocateVRegister(local.Type);
				tr.Instructions.Add(new IRInstruction(IROpCode.SX, r, ret));
				ret = r;
			}
			return ret;
		}
	}
}
