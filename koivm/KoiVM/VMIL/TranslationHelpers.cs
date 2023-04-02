using System;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.RT;

namespace KoiVM.VMIL
{
	// Token: 0x020000BB RID: 187
	public static class TranslationHelpers
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public static ILOpCode GetLIND(ASTType type, TypeSig rawType)
		{
			bool flag = rawType != null;
			ILOpCode ilopCode;
			if (flag)
			{
				switch (rawType.ElementType)
				{
				case ElementType.Boolean:
				case ElementType.I1:
				case ElementType.U1:
					return ILOpCode.LIND_BYTE;
				case ElementType.Char:
				case ElementType.I2:
				case ElementType.U2:
					return ILOpCode.LIND_WORD;
				case ElementType.I4:
				case ElementType.U4:
				case ElementType.R4:
					return ILOpCode.LIND_DWORD;
				case ElementType.I8:
				case ElementType.U8:
				case ElementType.R8:
					return ILOpCode.LIND_QWORD;
				case ElementType.Ptr:
				case ElementType.I:
				case ElementType.U:
					return ILOpCode.LIND_PTR;
				}
				ilopCode = ILOpCode.LIND_OBJECT;
			}
			else
			{
				switch (type)
				{
				case ASTType.I4:
				case ASTType.R4:
					return ILOpCode.LIND_DWORD;
				case ASTType.I8:
				case ASTType.R8:
					return ILOpCode.LIND_QWORD;
				case ASTType.Ptr:
					return ILOpCode.LIND_PTR;
				}
				ilopCode = ILOpCode.LIND_OBJECT;
			}
			return ilopCode;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000100D8 File Offset: 0x0000E2D8
		public static ILOpCode GetLIND(this IRRegister reg)
		{
			return TranslationHelpers.GetLIND(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001010C File Offset: 0x0000E30C
		public static ILOpCode GetLIND(this IRPointer ptr)
		{
			return TranslationHelpers.GetLIND(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010140 File Offset: 0x0000E340
		public static ILOpCode GetSIND(ASTType type, TypeSig rawType)
		{
			bool flag = rawType != null;
			ILOpCode ilopCode;
			if (flag)
			{
				switch (rawType.ElementType)
				{
				case ElementType.Boolean:
				case ElementType.I1:
				case ElementType.U1:
					return ILOpCode.SIND_BYTE;
				case ElementType.Char:
				case ElementType.I2:
				case ElementType.U2:
					return ILOpCode.SIND_WORD;
				case ElementType.I4:
				case ElementType.U4:
				case ElementType.R4:
					return ILOpCode.SIND_DWORD;
				case ElementType.I8:
				case ElementType.U8:
				case ElementType.R8:
					return ILOpCode.SIND_QWORD;
				case ElementType.Ptr:
				case ElementType.I:
				case ElementType.U:
					return ILOpCode.SIND_PTR;
				}
				ilopCode = ILOpCode.SIND_OBJECT;
			}
			else
			{
				switch (type)
				{
				case ASTType.I4:
				case ASTType.R4:
					return ILOpCode.SIND_DWORD;
				case ASTType.I8:
				case ASTType.R8:
					return ILOpCode.SIND_QWORD;
				case ASTType.Ptr:
					return ILOpCode.SIND_PTR;
				}
				ilopCode = ILOpCode.SIND_OBJECT;
			}
			return ilopCode;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010224 File Offset: 0x0000E424
		public static ILOpCode GetSIND(this IRRegister reg)
		{
			return TranslationHelpers.GetSIND(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00010258 File Offset: 0x0000E458
		public static ILOpCode GetSIND(this IRPointer ptr)
		{
			return TranslationHelpers.GetSIND(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001028C File Offset: 0x0000E48C
		public static ILOpCode GetPUSHR(ASTType type, TypeSig rawType)
		{
			bool flag = rawType != null;
			ILOpCode ilopCode;
			if (flag)
			{
				switch (rawType.ElementType)
				{
				case ElementType.Boolean:
				case ElementType.I1:
				case ElementType.U1:
					return ILOpCode.PUSHR_BYTE;
				case ElementType.Char:
				case ElementType.I2:
				case ElementType.U2:
					return ILOpCode.PUSHR_WORD;
				case ElementType.I4:
				case ElementType.U4:
				case ElementType.R4:
					return ILOpCode.PUSHR_DWORD;
				case ElementType.I8:
				case ElementType.U8:
				case ElementType.R8:
				case ElementType.Ptr:
					return ILOpCode.PUSHR_QWORD;
				}
				ilopCode = ILOpCode.PUSHR_OBJECT;
			}
			else
			{
				switch (type)
				{
				case ASTType.I4:
				case ASTType.R4:
					return ILOpCode.PUSHR_DWORD;
				case ASTType.I8:
				case ASTType.R8:
				case ASTType.Ptr:
					return ILOpCode.PUSHR_QWORD;
				}
				ilopCode = ILOpCode.PUSHR_OBJECT;
			}
			return ilopCode;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00010340 File Offset: 0x0000E540
		public static ILOpCode GetPUSHR(this IRRegister reg)
		{
			return TranslationHelpers.GetPUSHR(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00010374 File Offset: 0x0000E574
		public static ILOpCode GetPUSHR(this IRPointer ptr)
		{
			return TranslationHelpers.GetPUSHR(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000103A8 File Offset: 0x0000E5A8
		public static ILOpCode GetPUSHI(this ASTType type)
		{
			switch (type)
			{
			case ASTType.I4:
			case ASTType.R4:
				return ILOpCode.PUSHI_DWORD;
			case ASTType.I8:
			case ASTType.R8:
			case ASTType.Ptr:
				return ILOpCode.PUSHI_QWORD;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000103EC File Offset: 0x0000E5EC
		public static void PushOperand(this ILTranslator tr, IIROperand operand)
		{
			bool flag = operand is IRRegister;
			if (flag)
			{
				ILRegister reg = ILRegister.LookupRegister(((IRRegister)operand).Register);
				tr.Instructions.Add(new ILInstruction(((IRRegister)operand).GetPUSHR(), reg));
			}
			else
			{
				bool flag2 = operand is IRPointer;
				if (flag2)
				{
					IRPointer pointer = (IRPointer)operand;
					ILRegister reg2 = ILRegister.LookupRegister(pointer.Register.Register);
					tr.Instructions.Add(new ILInstruction(pointer.Register.GetPUSHR(), reg2));
					bool flag3 = pointer.Offset != 0;
					if (flag3)
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(pointer.Offset, ASTType.I4)));
						bool flag4 = pointer.Register.Type == ASTType.I4;
						if (flag4)
						{
							tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_DWORD));
						}
						else
						{
							tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_QWORD));
						}
					}
					tr.Instructions.Add(new ILInstruction(pointer.GetLIND()));
				}
				else
				{
					bool flag5 = operand is IRConstant;
					if (flag5)
					{
						IRConstant constant = (IRConstant)operand;
						bool flag6 = constant.Value == null;
						if (flag6)
						{
							tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(0, ASTType.O)));
						}
						else
						{
							tr.Instructions.Add(new ILInstruction(constant.Type.Value.GetPUSHI(), ILImmediate.Create(constant.Value, constant.Type.Value)));
						}
					}
					else
					{
						bool flag7 = operand is IRMetaTarget;
						if (flag7)
						{
							MethodDef method = (MethodDef)((IRMetaTarget)operand).MetadataItem;
							tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILMethodTarget(method)));
						}
						else
						{
							bool flag8 = operand is IRBlockTarget;
							if (flag8)
							{
								IBasicBlock target = ((IRBlockTarget)operand).Target;
								tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILBlockTarget(target)));
							}
							else
							{
								bool flag9 = operand is IRJumpTable;
								if (flag9)
								{
									IBasicBlock[] targets = ((IRJumpTable)operand).Targets;
									tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILJumpTable(targets)));
								}
								else
								{
									bool flag10 = operand is IRDataTarget;
									if (!flag10)
									{
										throw new NotSupportedException();
									}
									BinaryChunk target2 = ((IRDataTarget)operand).Target;
									tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILDataTarget(target2)));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001069C File Offset: 0x0000E89C
		public static void PopOperand(this ILTranslator tr, IIROperand operand)
		{
			bool flag = operand is IRRegister;
			if (flag)
			{
				ILRegister reg = ILRegister.LookupRegister(((IRRegister)operand).Register);
				tr.Instructions.Add(new ILInstruction(ILOpCode.POP, reg));
			}
			else
			{
				bool flag2 = operand is IRPointer;
				if (!flag2)
				{
					throw new NotSupportedException();
				}
				IRPointer pointer = (IRPointer)operand;
				ILRegister reg2 = ILRegister.LookupRegister(pointer.Register.Register);
				tr.Instructions.Add(new ILInstruction(pointer.Register.GetPUSHR(), reg2));
				bool flag3 = pointer.Offset != 0;
				if (flag3)
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(pointer.Offset, ASTType.I4)));
					bool flag4 = pointer.Register.Type == ASTType.I4;
					if (flag4)
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_DWORD));
					}
					else
					{
						tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_QWORD));
					}
				}
				tr.Instructions.Add(new ILInstruction(pointer.GetSIND()));
			}
		}
	}
}
