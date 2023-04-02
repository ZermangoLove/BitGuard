using System;
using System.Collections.Generic;
using System.Linq;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.VM;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000E7 RID: 231
	public class SaveRegistersTransform : IPostTransform
	{
		// Token: 0x0600036E RID: 878 RVA: 0x00002E37 File Offset: 0x00001037
		public void Initialize(ILPostTransformer tr)
		{
			this.saveRegs = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method).UsedRegister;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00002E60 File Offset: 0x00001060
		public void Transform(ILPostTransformer tr)
		{
			tr.Instructions.VisitInstrs<ILPostTransformer>(new VisitFunc<ILInstrList, ILInstruction, ILPostTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000121A8 File Offset: 0x000103A8
		private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILPostTransformer tr)
		{
			bool flag = instr.OpCode != ILOpCode.__BEGINCALL && instr.OpCode != ILOpCode.__ENDCALL;
			if (!flag)
			{
				InstrCallInfo callInfo = (InstrCallInfo)instr.Annotation;
				bool isECall = callInfo.IsECall;
				if (isECall)
				{
					instrs.RemoveAt(index);
					index--;
				}
				else
				{
					HashSet<VMRegisters> saving = new HashSet<VMRegisters>(this.saveRegs);
					IRVariable retVar = (IRVariable)callInfo.ReturnValue;
					bool flag2 = retVar != null;
					if (flag2)
					{
						bool flag3 = callInfo.ReturnSlot == null;
						if (flag3)
						{
							VMRegisters retReg = callInfo.ReturnRegister.Register;
							saving.Remove(retReg);
							bool flag4 = retReg > VMRegisters.R0;
							if (flag4)
							{
								saving.Add(VMRegisters.R0);
							}
						}
						else
						{
							saving.Add(VMRegisters.R0);
						}
					}
					else
					{
						saving.Add(VMRegisters.R0);
					}
					bool flag5 = instr.OpCode == ILOpCode.__BEGINCALL;
					if (flag5)
					{
						instrs.Replace(index, saving.Select((VMRegisters reg) => new ILInstruction(ILOpCode.PUSHR_OBJECT, ILRegister.LookupRegister(reg), instr)));
					}
					else
					{
						instrs.Replace(index, saving.Select((VMRegisters reg) => new ILInstruction(ILOpCode.POP, ILRegister.LookupRegister(reg), instr)).Reverse<ILInstruction>());
					}
					index--;
				}
			}
		}

		// Token: 0x04000168 RID: 360
		private HashSet<VMRegisters> saveRegs;
	}
}
