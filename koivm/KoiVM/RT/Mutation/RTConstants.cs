using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.VM;
using KoiVM.VMIL;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000FC RID: 252
	public class RTConstants
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x00003200 File Offset: 0x00001400
		private void AddField(string fieldName, int fieldValue)
		{
			this.constants[fieldName] = fieldValue;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00016F5C File Offset: 0x0001515C
		private void Conclude(Random random, IList<Instruction> instrs, TypeDef constType)
		{
			List<KeyValuePair<string, int>> constValues = this.constants.ToList<KeyValuePair<string, int>>();
			random.Shuffle(constValues);
			foreach (KeyValuePair<string, int> c in constValues)
			{
				instrs.Add(new Instruction(OpCodes.Ldnull));
				instrs.Add(new Instruction(OpCodes.Ldc_I4, c.Value));
				instrs.Add(new Instruction(OpCodes.Stfld, constType.FindField(RTMap.VMConstMap[c.Key])));
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00017018 File Offset: 0x00015218
		public int? GetConstant(string name)
		{
			int ret;
			bool flag = !this.constants.TryGetValue(name, out ret);
			int? num;
			if (flag)
			{
				num = null;
			}
			else
			{
				num = new int?(ret);
			}
			return num;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00017054 File Offset: 0x00015254
		public void InjectConstants(ModuleDef rtModule, VMDescriptor desc, RuntimeHelpers helpers)
		{
			TypeDef constants = rtModule.Find(RTMap.VMConstants, true);
			MethodDef cctor = constants.FindOrCreateStaticConstructor();
			IList<Instruction> instrs = cctor.Body.Instructions;
			instrs.Clear();
			for (int i = 0; i < 16; i++)
			{
				VMRegisters reg = (VMRegisters)i;
				byte regId = desc.Architecture.Registers[reg];
				string regField = reg.ToString();
				this.AddField(regField, (int)regId);
			}
			for (int j = 0; j < 8; j++)
			{
				VMFlags fl = (VMFlags)j;
				int flId = desc.Architecture.Flags[fl];
				string flField = fl.ToString();
				this.AddField(flField, 1 << flId);
			}
			for (int k = 0; k < 68; k++)
			{
				ILOpCode op = (ILOpCode)k;
				byte opId = desc.Architecture.OpCodes[op];
				string opField = op.ToString();
				this.AddField(opField, (int)opId);
			}
			for (int l = 0; l < 17; l++)
			{
				VMCalls vc = (VMCalls)l;
				int vcId = desc.Runtime.VMCall[vc];
				string vcField = vc.ToString();
				this.AddField(vcField, vcId);
			}
			this.AddField(ConstantFields.E_CALL.ToString(), (int)desc.Runtime.VCallOps.ECALL_CALL);
			this.AddField(ConstantFields.E_CALLVIRT.ToString(), (int)desc.Runtime.VCallOps.ECALL_CALLVIRT);
			this.AddField(ConstantFields.E_NEWOBJ.ToString(), (int)desc.Runtime.VCallOps.ECALL_NEWOBJ);
			this.AddField(ConstantFields.E_CALLVIRT_CONSTRAINED.ToString(), (int)desc.Runtime.VCallOps.ECALL_CALLVIRT_CONSTRAINED);
			this.AddField(ConstantFields.INIT.ToString(), (int)helpers.INIT);
			this.AddField(ConstantFields.INSTANCE.ToString(), (int)desc.Runtime.RTFlags.INSTANCE);
			this.AddField(ConstantFields.CATCH.ToString(), (int)desc.Runtime.RTFlags.EH_CATCH);
			this.AddField(ConstantFields.FILTER.ToString(), (int)desc.Runtime.RTFlags.EH_FILTER);
			this.AddField(ConstantFields.FAULT.ToString(), (int)desc.Runtime.RTFlags.EH_FAULT);
			this.AddField(ConstantFields.FINALLY.ToString(), (int)desc.Runtime.RTFlags.EH_FINALLY);
			this.Conclude(desc.Random, instrs, constants);
			instrs.Add(Instruction.Create(OpCodes.Ret));
			cctor.Body.OptimizeMacros();
		}

		// Token: 0x040001A5 RID: 421
		private Dictionary<string, int> constants = new Dictionary<string, int>();
	}
}
