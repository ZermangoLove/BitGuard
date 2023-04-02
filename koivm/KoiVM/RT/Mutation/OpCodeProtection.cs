using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000F9 RID: 249
	internal class OpCodeProtection
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x00003190 File Offset: 0x00001390
		public static void Execute(MethodDef method)
		{
			OpCodeProtection.LdfldProtection(method);
			OpCodeProtection.CallvirtProtection(method);
			OpCodeProtection.CtorCallProtection(method);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00016614 File Offset: 0x00014814
		private static void CtorCallProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				bool flag = instr[i].OpCode == OpCodes.Call && instr[i].Operand.ToString().ToLower().Contains("void") && i - 1 > 0 && instr[i - 1].IsLdarg();
				if (flag)
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Bne_Un_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000167C4 File Offset: 0x000149C4
		private static void LdfldProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				bool flag = instr[i].OpCode == OpCodes.Ldfld && i - 1 > 0 && instr[i - 1].IsLdarg();
				if (flag)
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00016950 File Offset: 0x00014B50
		private static void CallvirtProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				bool flag = instr[i].OpCode == OpCodes.Callvirt && instr[i].Operand.ToString().ToLower().Contains("int32") && i - 1 > 0 && instr[i - 1].IsLdloc();
				if (flag)
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x0400019F RID: 415
		private static Random random = new Random();
	}
}
