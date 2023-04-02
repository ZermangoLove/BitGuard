using System;
using System.Reflection;
using System.Runtime.InteropServices;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000068 RID: 104
	[StructLayout(LayoutKind.Explicit)]
	internal struct VMSlot
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000095C8 File Offset: 0x000077C8
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000095E0 File Offset: 0x000077E0
		public ulong U8
		{
			get
			{
				return this.u8;
			}
			set
			{
				this.u8 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000095F4 File Offset: 0x000077F4
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000960C File Offset: 0x0000780C
		public uint U4
		{
			get
			{
				return this.u4;
			}
			set
			{
				this.u4 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00009620 File Offset: 0x00007820
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00009638 File Offset: 0x00007838
		public ushort U2
		{
			get
			{
				return this.u2;
			}
			set
			{
				this.u2 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000964C File Offset: 0x0000784C
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00009664 File Offset: 0x00007864
		public byte U1
		{
			get
			{
				return this.u1;
			}
			set
			{
				this.u1 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00009678 File Offset: 0x00007878
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00009690 File Offset: 0x00007890
		public double R8
		{
			get
			{
				return this.r8;
			}
			set
			{
				this.r8 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000096A4 File Offset: 0x000078A4
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000096BC File Offset: 0x000078BC
		public float R4
		{
			get
			{
				return this.r4;
			}
			set
			{
				this.r4 = value;
				this.o = null;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000096D0 File Offset: 0x000078D0
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000096E8 File Offset: 0x000078E8
		public object O
		{
			get
			{
				return this.o;
			}
			set
			{
				this.o = value;
				this.u8 = 0UL;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000096FC File Offset: 0x000078FC
		public static VMSlot FromObject(object obj, Type type)
		{
			bool isEnum = type.IsEnum;
			VMSlot vmslot;
			if (isEnum)
			{
				Type elemType = Enum.GetUnderlyingType(type);
				vmslot = VMSlot.FromObject(Convert.ChangeType(obj, elemType), elemType);
			}
			else
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u1 = (((bool)obj) ? 1 : 0)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Char:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u2 = (ushort)((char)obj)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.SByte:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u1 = (byte)((sbyte)obj)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Byte:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u1 = (byte)obj
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Int16:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u2 = (ushort)((short)obj)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.UInt16:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u2 = (ushort)obj
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Int32:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u4 = (uint)((int)obj)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.UInt32:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u4 = (uint)obj
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Int64:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u8 = (ulong)((long)obj)
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.UInt64:
				{
					VMSlot vmslot2 = new VMSlot
					{
						u8 = (ulong)obj
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Single:
				{
					VMSlot vmslot2 = new VMSlot
					{
						r4 = (float)obj
					};
					vmslot = vmslot2;
					break;
				}
				case TypeCode.Double:
				{
					VMSlot vmslot2 = new VMSlot
					{
						r8 = (double)obj
					};
					vmslot = vmslot2;
					break;
				}
				default:
				{
					bool flag = obj is Pointer;
					if (flag)
					{
						VMSlot vmslot2 = new VMSlot
						{
							u8 = Pointer.Unbox(obj)
						};
						vmslot = vmslot2;
					}
					else
					{
						bool flag2 = obj is IntPtr;
						if (flag2)
						{
							VMSlot vmslot2 = new VMSlot
							{
								u8 = (ulong)(long)((IntPtr)obj)
							};
							vmslot = vmslot2;
						}
						else
						{
							bool flag3 = obj is UIntPtr;
							if (flag3)
							{
								VMSlot vmslot2 = new VMSlot
								{
									u8 = (ulong)((UIntPtr)obj)
								};
								vmslot = vmslot2;
							}
							else
							{
								bool isValueType = type.IsValueType;
								if (isValueType)
								{
									VMSlot vmslot2 = new VMSlot
									{
										o = ValueTypeBox.Box(obj, type)
									};
									vmslot = vmslot2;
								}
								else
								{
									VMSlot vmslot2 = new VMSlot
									{
										o = obj
									};
									vmslot = vmslot2;
								}
							}
						}
					}
					break;
				}
				}
			}
			return vmslot;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000099A2 File Offset: 0x00007BA2
		public unsafe void ToTypedReferencePrimitive(TypedRefPtr typedRef)
		{
			*(TypedReference*)typedRef = __makeref(this.u4);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000099BC File Offset: 0x00007BBC
		public unsafe void ToTypedReferenceObject(TypedRefPtr typedRef, Type type)
		{
			bool flag = this.o is ValueType && type.IsValueType;
			if (flag)
			{
				TypedReferenceHelpers.UnboxTypedRef(this.o, typedRef);
			}
			else
			{
				*(TypedReference*)typedRef = __makeref(this.o);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009A0C File Offset: 0x00007C0C
		public object ToObject(Type type)
		{
			bool isEnum = type.IsEnum;
			object obj;
			if (isEnum)
			{
				obj = Enum.ToObject(type, this.ToObject(Enum.GetUnderlyingType(type)));
			}
			else
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
					obj = this.u1 > 0;
					break;
				case TypeCode.Char:
					obj = (char)this.u2;
					break;
				case TypeCode.SByte:
					obj = (sbyte)this.u1;
					break;
				case TypeCode.Byte:
					obj = this.u1;
					break;
				case TypeCode.Int16:
					obj = (short)this.u2;
					break;
				case TypeCode.UInt16:
					obj = this.u2;
					break;
				case TypeCode.Int32:
					obj = (int)this.u4;
					break;
				case TypeCode.UInt32:
					obj = this.u4;
					break;
				case TypeCode.Int64:
					obj = (long)this.u8;
					break;
				case TypeCode.UInt64:
					obj = this.u8;
					break;
				case TypeCode.Single:
					obj = this.r4;
					break;
				case TypeCode.Double:
					obj = this.r8;
					break;
				default:
				{
					bool isPointer = type.IsPointer;
					if (isPointer)
					{
						obj = Pointer.Box(this.u8, type);
					}
					else
					{
						bool flag = type == typeof(IntPtr);
						if (flag)
						{
							obj = (Platform.x64 ? new IntPtr((long)this.u8) : new IntPtr((int)this.u4));
						}
						else
						{
							bool flag2 = type == typeof(UIntPtr);
							if (flag2)
							{
								obj = (Platform.x64 ? new UIntPtr(this.u8) : new UIntPtr(this.u4));
							}
							else
							{
								obj = ValueTypeBox.Unbox(this.o);
							}
						}
					}
					break;
				}
				}
			}
			return obj;
		}

		// Token: 0x0400002E RID: 46
		[FieldOffset(0)]
		private ulong u8;

		// Token: 0x0400002F RID: 47
		[FieldOffset(0)]
		private double r8;

		// Token: 0x04000030 RID: 48
		[FieldOffset(0)]
		private uint u4;

		// Token: 0x04000031 RID: 49
		[FieldOffset(0)]
		private float r4;

		// Token: 0x04000032 RID: 50
		[FieldOffset(0)]
		private ushort u2;

		// Token: 0x04000033 RID: 51
		[FieldOffset(0)]
		private byte u1;

		// Token: 0x04000034 RID: 52
		[FieldOffset(8)]
		private object o;

		// Token: 0x04000035 RID: 53
		public static readonly VMSlot Null;
	}
}
