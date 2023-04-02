using System;
using System.Collections.Generic;
using System.IO;
using dnlib.DotNet;
using KoiVM.AST.IR;
using KoiVM.VM;

namespace KoiVM
{
	// Token: 0x02000007 RID: 7
	public static class Utils
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00004D98 File Offset: 0x00002F98
		public static void Increment<T>(this Dictionary<T, int> self, T key)
		{
			int count;
			bool flag = !self.TryGetValue(key, out count);
			if (flag)
			{
				count = 0;
			}
			count = (self[key] = count + 1);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public static void Shuffle<T>(this Random random, IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int j = random.Next(i + 1);
				T value = list[j];
				list[j] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000021E5 File Offset: 0x000003E5
		public static void Replace<T>(this List<T> list, int index, IEnumerable<T> newItems)
		{
			list.RemoveAt(index);
			list.InsertRange(index, newItems);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004E18 File Offset: 0x00003018
		public static void Replace(this List<IRInstruction> list, int index, IEnumerable<IRInstruction> newItems)
		{
			IRInstruction instr = list[index];
			list.RemoveAt(index);
			foreach (IRInstruction i in newItems)
			{
				i.ILAST = instr.ILAST;
			}
			list.InsertRange(index, newItems);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004E84 File Offset: 0x00003084
		public static bool IsGPR(this VMRegisters reg)
		{
			return reg >= VMRegisters.R0 && reg <= VMRegisters.R7;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004EB0 File Offset: 0x000030B0
		public static uint GetCompressedUIntLength(uint value)
		{
			uint len = 0U;
			do
			{
				value >>= 7;
				len += 1U;
			}
			while (value > 0U);
			return len;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004ED8 File Offset: 0x000030D8
		public static void WriteCompressedUInt(this BinaryWriter writer, uint value)
		{
			do
			{
				byte b = (byte)(value & 127U);
				value >>= 7;
				bool flag = value > 0U;
				if (flag)
				{
					b |= 128;
				}
				writer.Write(b);
			}
			while (value > 0U);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004F14 File Offset: 0x00003114
		public static TypeSig ResolveType(this GenericArguments genericArgs, TypeSig typeSig)
		{
			ElementType elementType = typeSig.ElementType;
			ElementType elementType2 = elementType;
			switch (elementType2)
			{
			case ElementType.Ptr:
				return new PtrSig(genericArgs.ResolveType(typeSig.Next));
			case ElementType.ByRef:
				return new ByRefSig(genericArgs.ResolveType(typeSig.Next));
			case ElementType.ValueType:
			case ElementType.Class:
			case ElementType.TypedByRef:
			case ElementType.I:
			case ElementType.U:
			case ElementType.R:
			case ElementType.FnPtr:
			case ElementType.Object:
				break;
			case ElementType.Var:
			case ElementType.MVar:
				return genericArgs.Resolve(typeSig);
			case ElementType.Array:
			{
				ArraySig arraySig = (ArraySig)typeSig;
				return new ArraySig(genericArgs.ResolveType(typeSig.Next), arraySig.Rank, arraySig.Sizes, arraySig.LowerBounds);
			}
			case ElementType.GenericInst:
			{
				GenericInstSig genInst = (GenericInstSig)typeSig;
				List<TypeSig> typeArgs = new List<TypeSig>();
				foreach (TypeSig arg in genInst.GenericArguments)
				{
					typeArgs.Add(genericArgs.ResolveType(arg));
				}
				return new GenericInstSig(genInst.GenericType, typeArgs);
			}
			case ElementType.ValueArray:
				return new ValueArraySig(genericArgs.ResolveType(typeSig.Next), ((ValueArraySig)typeSig).Size);
			case ElementType.SZArray:
				return new SZArraySig(genericArgs.ResolveType(typeSig.Next));
			case ElementType.CModReqd:
				return new CModReqdSig(((CModReqdSig)typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));
			case ElementType.CModOpt:
				return new CModOptSig(((CModOptSig)typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));
			default:
				if (elementType2 == ElementType.Module)
				{
					return new ModuleSig(((ModuleSig)typeSig).Index, genericArgs.ResolveType(typeSig.Next));
				}
				if (elementType2 == ElementType.Pinned)
				{
					return new PinnedSig(genericArgs.ResolveType(typeSig.Next));
				}
				break;
			}
			bool isTypeDefOrRef = typeSig.IsTypeDefOrRef;
			if (isTypeDefOrRef)
			{
				TypeDefOrRefSig s = (TypeDefOrRefSig)typeSig;
				bool flag = s.TypeDefOrRef is TypeSpec;
				if (flag)
				{
					throw new NotSupportedException();
				}
			}
			return typeSig;
		}
	}
}
