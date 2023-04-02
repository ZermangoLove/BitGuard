using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A3 RID: 163
	public class InitLocalTransform : ITransform
	{
		// Token: 0x0600026D RID: 621 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E814 File Offset: 0x0000CA14
		public void Transform(IRTransformer tr)
		{
			bool flag = !tr.Context.Method.Body.InitLocals;
			if (!flag)
			{
				tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E85C File Offset: 0x0000CA5C
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode == IROpCode.__ENTRY && !this.done;
			if (flag)
			{
				List<IRInstruction> init = new List<IRInstruction>();
				init.Add(instr);
				foreach (Local local in tr.Context.Method.Body.Variables)
				{
					bool flag2 = local.Type.IsValueType && !local.Type.IsPrimitive;
					if (flag2)
					{
						IRVariable adr = tr.Context.AllocateVRegister(ASTType.ByRef);
						init.Add(new IRInstruction(IROpCode.__LEA, adr, tr.Context.ResolveLocal(local)));
						int typeId = (int)tr.VM.Data.GetId(local.Type.RemovePinnedAndModifiers().ToTypeDefOrRef());
						int ecallId = tr.VM.Runtime.VMCall.INITOBJ;
						init.Add(new IRInstruction(IROpCode.PUSH, adr));
						init.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(ecallId), IRConstant.FromI4(typeId)));
					}
				}
				instrs.Replace(index, init);
				this.done = true;
			}
		}

		// Token: 0x040000E6 RID: 230
		private bool done;
	}
}
