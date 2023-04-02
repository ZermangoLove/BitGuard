using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000F7 RID: 247
	public class MutateRT
	{
		// Token: 0x060003D4 RID: 980 RVA: 0x000146EC File Offset: 0x000128EC
		public static void IntControlFlow(MethodDef method)
		{
			Local local = new Local(method.Module.ImportAsTypeSig(typeof(int)));
			method.Body.Variables.Add(local);
			for (int i = 0; i < method.Body.Instructions.Count; i++)
			{
				bool flag = method.Body.Instructions[i].IsLdcI4();
				if (flag)
				{
					int numorig = MutateRT.rnd.Next();
					int div = MutateRT.rnd.Next();
					int num = numorig ^ div;
					Instruction nop = OpCodes.Nop.ToInstruction();
					method.Body.Instructions.Insert(i + 1, OpCodes.Stloc_S.ToInstruction(local));
					method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - 4));
					method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
					method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
					method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
					method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
					method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
					method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
					method.Body.Instructions.Insert(i + 9, OpCodes.Stloc_S.ToInstruction(local));
					method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Sizeof, method.Module.Import(typeof(float))));
					method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
					method.Body.Instructions.Insert(i + 12, nop);
					i += 12;
				}
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00014938 File Offset: 0x00012B38
		public static void IntControlFlow_NoSizeOf(MethodDef method)
		{
			Local local = new Local(method.Module.ImportAsTypeSig(typeof(int)));
			method.Body.Variables.Add(local);
			for (int i = 0; i < method.Body.Instructions.Count; i++)
			{
				bool flag = method.Body.Instructions[i].IsLdcI4();
				if (flag)
				{
					int numorig = MutateRT.rnd.Next();
					int div = MutateRT.rnd.Next();
					int num = numorig ^ div;
					Instruction nop = OpCodes.Nop.ToInstruction();
					method.Body.Instructions.Insert(i + 1, OpCodes.Stloc_S.ToInstruction(local));
					method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - 4));
					method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
					method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
					method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
					method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
					method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
					method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
					method.Body.Instructions.Insert(i + 9, OpCodes.Stloc_S.ToInstruction(local));
					method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Ldc_I4, 4));
					method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
					method.Body.Instructions.Insert(i + 12, nop);
					i += 12;
				}
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00014B70 File Offset: 0x00012D70
		public static void BasicEncodeInt(MethodDef method)
		{
			bool flag = method.HasBody && method.Body.HasInstructions;
			if (flag)
			{
				int x = 0;
				while (x < method.Body.Instructions.Count)
				{
					bool flag2 = method.Body.Instructions[x].IsLdcI4();
					if (flag2)
					{
						uint num = (uint)method.Body.Instructions[x].GetLdcI4Value();
						uint num2 = (uint)MutateRT.rnd.Next();
						uint value = num2 ^ num;
						method.Body.Instructions[x].OpCode = OpCodes.Ldc_I4;
						method.Body.Instructions[x].Operand = (int)value;
						method.Body.Instructions.Insert(x + 1, Instruction.Create(OpCodes.Ldc_I4, (int)num2));
						method.Body.Instructions.Insert(x + 2, Instruction.Create(OpCodes.Xor));
						x += 3;
					}
					else
					{
						x++;
					}
				}
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00014C8C File Offset: 0x00012E8C
		public static void Array_Mutation(MethodDef a)
		{
			a.Body.Instructions.SimplifyBranches();
			a.Body.Instructions.SimplifyMacros(a.Body.Variables, a.Parameters);
			List<Instruction> list = new List<Instruction>();
			Local local = new Local(new SZArraySig(a.Module.CorLibTypes.Object));
			Local local2 = new Local(a.Module.CorLibTypes.Object);
			Local local3 = new Local(new SZArraySig(a.Module.CorLibTypes.Int32));
			Local local4 = new Local(a.Module.CorLibTypes.Int32);
			list.Add(OpCodes.Ldc_I4.ToInstruction(a.Body.Variables.Count));
			list.Add(OpCodes.Newarr.ToInstruction(a.Module.CorLibTypes.Int32));
			list.Add(OpCodes.Stloc.ToInstruction(local3));
			for (int i = 0; i < a.Body.Variables.Count; i++)
			{
				list.Add(OpCodes.Ldloc.ToInstruction(local3));
				list.Add(OpCodes.Ldc_I4.ToInstruction(i));
				list.Add(OpCodes.Ldc_I4_M1.ToInstruction());
				list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
			}
			list.Add(OpCodes.Ldc_I4_0.ToInstruction());
			list.Add(OpCodes.Stloc.ToInstruction(local4));
			for (int j = 0; j < a.Body.Instructions.Count; j++)
			{
				Instruction instruction = a.Body.Instructions[j];
				list.Add(instruction);
				bool flag = instruction.OpCode.Code != Code.Ldloc && instruction.OpCode.Code != Code.Ldloca;
				if (flag)
				{
					bool flag2 = instruction.OpCode.Code == Code.Stloc;
					if (flag2)
					{
						Local local5 = (Local)instruction.Operand;
						int index = local5.Index;
						bool isValueType = local5.Type.IsValueType;
						if (isValueType)
						{
							list.Add(OpCodes.Box.ToInstruction(local5.Type.ToTypeDefOrRef()));
						}
						else
						{
							list.Add(OpCodes.Castclass.ToInstruction(a.Module.CorLibTypes.Object));
						}
						Instruction instruction2 = OpCodes.Nop.ToInstruction();
						list.Add(OpCodes.Ldloc.ToInstruction(local3));
						list.Add(OpCodes.Ldc_I4.ToInstruction(index));
						list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
						list.Add(OpCodes.Ldc_I4_M1.ToInstruction());
						list.Add(OpCodes.Ceq.ToInstruction());
						list.Add(OpCodes.Brtrue.ToInstruction(instruction2));
						list.Add(OpCodes.Ldloc.ToInstruction(local));
						list.Add(OpCodes.Ldloc.ToInstruction(local3));
						list.Add(OpCodes.Ldc_I4.ToInstruction(index));
						list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
						list.Add(OpCodes.Ldnull.ToInstruction());
						list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Object));
						list.Add(instruction2);
						list.Add(OpCodes.Ldloc.ToInstruction(local4));
						list.Add(OpCodes.Ldc_I4_1.ToInstruction());
						list.Add(OpCodes.Add.ToInstruction());
						list.Add(OpCodes.Stloc.ToInstruction(local4));
						list.Add(OpCodes.Ldloc.ToInstruction(local3));
						list.Add(OpCodes.Ldc_I4.ToInstruction(index));
						list.Add(OpCodes.Ldloc.ToInstruction(local4));
						list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
						list.Add(OpCodes.Stloc.ToInstruction(local2));
						list.Add(OpCodes.Ldloc.ToInstruction(local));
						list.Add(OpCodes.Ldloc.ToInstruction(local3));
						list.Add(OpCodes.Ldc_I4.ToInstruction(index));
						list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
						list.Add(OpCodes.Ldloc.ToInstruction(local2));
						list.Add(OpCodes.Stelem_Ref.ToInstruction());
						list.Add(OpCodes.Ldnull.ToInstruction());
						list.Add(OpCodes.Stloc.ToInstruction(local2));
						instruction.Operand = null;
						instruction.OpCode = OpCodes.Nop;
					}
				}
				else
				{
					Local local6 = (Local)instruction.Operand;
					int index2 = local6.Index;
					list.Add(OpCodes.Ldloc.ToInstruction(local));
					list.Add(OpCodes.Ldloc.ToInstruction(local3));
					list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
					list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
					list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Object));
					list.Add(OpCodes.Dup.ToInstruction());
					list.Add(OpCodes.Ldloc.ToInstruction(local));
					list.Add(OpCodes.Ldloc.ToInstruction(local3));
					list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
					list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
					list.Add(OpCodes.Ldnull.ToInstruction());
					list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Object));
					list.Add(OpCodes.Ldloc.ToInstruction(local4));
					list.Add(OpCodes.Ldc_I4_1.ToInstruction());
					list.Add(OpCodes.Add.ToInstruction());
					list.Add(OpCodes.Stloc.ToInstruction(local4));
					list.Add(OpCodes.Ldloc.ToInstruction(local3));
					list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
					list.Add(OpCodes.Ldloc.ToInstruction(local4));
					list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
					list.Add(OpCodes.Stloc.ToInstruction(local2));
					list.Add(OpCodes.Ldloc.ToInstruction(local));
					list.Add(OpCodes.Ldloc.ToInstruction(local3));
					list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
					list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
					list.Add(OpCodes.Ldloc.ToInstruction(local2));
					list.Add(OpCodes.Stelem_Ref.ToInstruction());
					list.Add(OpCodes.Ldnull.ToInstruction());
					list.Add(OpCodes.Stloc.ToInstruction(local2));
					bool isValueType2 = local6.Type.IsValueType;
					if (isValueType2)
					{
						bool flag3 = instruction.OpCode.Code == Code.Ldloc;
						if (flag3)
						{
							list.Add(OpCodes.Unbox_Any.ToInstruction(local6.Type.ToTypeDefOrRef()));
						}
						else
						{
							list.Add(OpCodes.Unbox.ToInstruction(local6.Type.ToTypeDefOrRef()));
						}
					}
					else
					{
						list.Add(OpCodes.Castclass.ToInstruction(local6.Type.ToTypeDefOrRef()));
					}
					instruction.Operand = null;
					instruction.OpCode = OpCodes.Nop;
				}
			}
			a.Body.Variables.Clear();
			a.Body.Variables.Add(local);
			a.Body.Variables.Add(local2);
			a.Body.Variables.Add(local3);
			a.Body.Variables.Add(local4);
			list.Insert(0, OpCodes.Ldc_I4.ToInstruction(new Random().Next(10000, 20000)));
			list.Insert(1, OpCodes.Newarr.ToInstruction(a.Module.CorLibTypes.Object));
			list.Insert(2, OpCodes.Stloc.ToInstruction(local));
			list.OptimizeBranches();
			list.OptimizeMacros();
			a.Body.Instructions.Clear();
			foreach (Instruction item in list)
			{
				a.Body.Instructions.Add(item);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015604 File Offset: 0x00013804
		public static void EncodeIntSizeOf(MethodDef method)
		{
			Random R = new Random();
			int num = 0;
			ITypeDefOrRef type = null;
			method.Body.SimplifyBranches();
			for (int i = 0; i < method.Body.Instructions.Count; i++)
			{
				Instruction instruction = method.Body.Instructions[i];
				bool flag2 = instruction.IsLdcI4();
				if (flag2)
				{
					switch (R.Next(1, 16))
					{
					case 1:
						type = method.Module.Import(typeof(int));
						num = 4;
						break;
					case 2:
						type = method.Module.Import(typeof(sbyte));
						num = 1;
						break;
					case 3:
						type = method.Module.Import(typeof(byte));
						num = 1;
						break;
					case 4:
						type = method.Module.Import(typeof(bool));
						num = 1;
						break;
					case 5:
						type = method.Module.Import(typeof(decimal));
						num = 16;
						break;
					case 6:
						type = method.Module.Import(typeof(short));
						num = 2;
						break;
					case 7:
						type = method.Module.Import(typeof(long));
						num = 8;
						break;
					case 8:
						type = method.Module.Import(typeof(uint));
						num = 4;
						break;
					case 9:
						type = method.Module.Import(typeof(float));
						num = 4;
						break;
					case 10:
						type = method.Module.Import(typeof(char));
						num = 2;
						break;
					case 11:
						type = method.Module.Import(typeof(ushort));
						num = 2;
						break;
					case 12:
						type = method.Module.Import(typeof(double));
						num = 8;
						break;
					case 13:
						type = method.Module.Import(typeof(DateTime));
						num = 8;
						break;
					case 14:
						type = method.Module.Import(typeof(ConsoleKeyInfo));
						num = 12;
						break;
					case 15:
						type = method.Module.Import(typeof(Guid));
						num = 16;
						break;
					}
					int num2 = R.Next(1, 1000);
					bool flag = Convert.ToBoolean(R.Next(0, 2));
					switch ((num != 0) ? ((Convert.ToInt32(instruction.Operand) % num == 0) ? R.Next(1, 5) : R.Next(1, 4)) : R.Next(1, 4))
					{
					case 1:
						method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
						method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
						instruction.Operand = Convert.ToInt32(instruction.Operand) - num + (flag ? (-num2) : num2);
						method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
						method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
						i += 4;
						break;
					case 2:
						method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
						method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sub));
						instruction.Operand = Convert.ToInt32(instruction.Operand) + num + (flag ? (-num2) : num2);
						method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
						method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
						i += 4;
						break;
					case 3:
						method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
						method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
						instruction.Operand = Convert.ToInt32(instruction.Operand) - num + (flag ? (-num2) : num2);
						method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
						method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
						i += 4;
						break;
					case 4:
						method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
						method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Mul));
						instruction.Operand = Convert.ToInt32(instruction.Operand) / num;
						i += 2;
						break;
					default:
						method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
						method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
						i += 4;
						break;
					}
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00015BA0 File Offset: 0x00013DA0
		private static double RandomDouble(double min, double max)
		{
			return new Random().NextDouble() * (max - min) + min;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015BC4 File Offset: 0x00013DC4
		public static void SizeOfMutate(MethodDef methodDef)
		{
			bool hasBody = methodDef.HasBody;
			if (hasBody)
			{
				for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
				{
					bool flag = methodDef.Body.Instructions[i].IsLdcI4();
					if (flag)
					{
						MutateRT.body = methodDef.Body;
						int ldcI4Value = MutateRT.body.Instructions[i].GetLdcI4Value();
						int num = MutateRT.rnd.Next(1, 4);
						int num2 = ldcI4Value - num;
						MutateRT.body.Instructions[i].Operand = num2;
						MutateRT.Mutate(i, num, num2, methodDef.Module);
					}
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00015C88 File Offset: 0x00013E88
		private static void Mutate(int i, int sub, int num2, ModuleDef module)
		{
			switch (sub)
			{
			case 1:
				MutateRT.body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
				MutateRT.body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
				break;
			case 2:
				MutateRT.body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
				MutateRT.body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
				MutateRT.body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Add));
				MutateRT.body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));
				break;
			case 3:
				MutateRT.body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(int))));
				MutateRT.body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
				MutateRT.body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Sub));
				MutateRT.body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));
				break;
			case 4:
				MutateRT.body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(decimal))));
				MutateRT.body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(GCCollectionMode))));
				MutateRT.body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Sub));
				MutateRT.body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(int))));
				MutateRT.body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Sub));
				MutateRT.body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Add));
				break;
			}
		}

		// Token: 0x04000196 RID: 406
		private static Random rnd = new Random();

		// Token: 0x04000197 RID: 407
		private static CilBody body;
	}
}
