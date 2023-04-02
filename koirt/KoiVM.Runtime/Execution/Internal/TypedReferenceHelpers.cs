using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x02000071 RID: 113
	internal static class TypedReferenceHelpers
	{
		// Token: 0x06000184 RID: 388 RVA: 0x0000B000 File Offset: 0x00009200
		public unsafe static void CastTypedRef(TypedRefPtr typedRef, Type targetType)
		{
			Type sourceType = TypedReference.GetTargetType(*(TypedReference*)typedRef);
			KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(sourceType, targetType);
			object helper = TypedReferenceHelpers.castHelpers[key];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = TypedReferenceHelpers.castHelpers;
				lock (hashtable)
				{
					helper = TypedReferenceHelpers.castHelpers[key];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = TypedReferenceHelpers.BuildCastHelper(sourceType, targetType);
						TypedReferenceHelpers.castHelpers[key] = helper;
					}
				}
			}
			((TypedReferenceHelpers.Cast)helper)(typedRef);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public unsafe static void MakeTypedRef(void* ptr, TypedRefPtr typedRef, Type targetType)
		{
			object helper = TypedReferenceHelpers.makeHelpers[targetType];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = TypedReferenceHelpers.makeHelpers;
				lock (hashtable)
				{
					helper = TypedReferenceHelpers.makeHelpers[targetType];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = TypedReferenceHelpers.BuildMakeHelper(targetType);
						TypedReferenceHelpers.makeHelpers[targetType] = helper;
					}
				}
			}
			((TypedReferenceHelpers.Make)helper)(ptr, typedRef);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000B13C File Offset: 0x0000933C
		public static void UnboxTypedRef(object box, TypedRefPtr typedRef)
		{
			TypedReferenceHelpers.UnboxTypedRef(box, typedRef, box.GetType());
			bool flag = box is IValueTypeBox;
			if (flag)
			{
				TypedReferenceHelpers.CastTypedRef(typedRef, ((IValueTypeBox)box).GetValueType());
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000B178 File Offset: 0x00009378
		public static void UnboxTypedRef(object box, TypedRefPtr typedRef, Type boxType)
		{
			object helper = TypedReferenceHelpers.unboxHelpers[boxType];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = TypedReferenceHelpers.unboxHelpers;
				lock (hashtable)
				{
					helper = TypedReferenceHelpers.unboxHelpers[boxType];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = TypedReferenceHelpers.BuildUnboxHelper(boxType);
						TypedReferenceHelpers.unboxHelpers[boxType] = helper;
					}
				}
			}
			((TypedReferenceHelpers.Unbox)helper)(box, typedRef);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B200 File Offset: 0x00009400
		public unsafe static void SetTypedRef(object value, TypedRefPtr typedRef)
		{
			Type type = TypedReference.GetTargetType(*(TypedReference*)typedRef);
			object helper = TypedReferenceHelpers.setHelpers[type];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = TypedReferenceHelpers.setHelpers;
				lock (hashtable)
				{
					helper = TypedReferenceHelpers.setHelpers[type];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = TypedReferenceHelpers.BuildSetHelper(type);
						TypedReferenceHelpers.setHelpers[type] = helper;
					}
				}
			}
			((TypedReferenceHelpers.Set)helper)(value, typedRef);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B298 File Offset: 0x00009498
		public unsafe static void GetFieldAddr(VMContext context, object obj, FieldInfo field, TypedRefPtr typedRef)
		{
			object helper = TypedReferenceHelpers.fieldAddrHelpers[field];
			bool flag = helper == null;
			if (flag)
			{
				Hashtable hashtable = TypedReferenceHelpers.fieldAddrHelpers;
				lock (hashtable)
				{
					helper = TypedReferenceHelpers.fieldAddrHelpers[field];
					bool flag2 = helper == null;
					if (flag2)
					{
						helper = TypedReferenceHelpers.BuildAddrHelper(field);
						TypedReferenceHelpers.fieldAddrHelpers[field] = helper;
					}
				}
			}
			bool flag3 = obj == null;
			TypedReference objRef;
			if (flag3)
			{
				objRef = default(TypedReference);
			}
			else
			{
				bool flag4 = obj is IReference;
				if (flag4)
				{
					((IReference)obj).ToTypedReference(context, (void*)(&objRef), field.DeclaringType);
				}
				else
				{
					objRef = __makeref(obj);
					TypedReferenceHelpers.CastTypedRef((void*)(&objRef), obj.GetType());
				}
			}
			((TypedReferenceHelpers.FieldAdr)helper)((void*)(&objRef), typedRef);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000B388 File Offset: 0x00009588
		private static TypedReferenceHelpers.Cast BuildCastHelper(Type sourceType, Type targetType)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[] { typeof(TypedRefPtr) }, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldarga, 0);
			gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
			gen.Emit(OpCodes.Dup);
			gen.Emit(OpCodes.Ldobj, typeof(TypedReference));
			gen.Emit(OpCodes.Refanyval, sourceType);
			gen.Emit(OpCodes.Mkrefany, targetType);
			gen.Emit(OpCodes.Stobj, typeof(TypedReference));
			gen.Emit(OpCodes.Ret);
			return (TypedReferenceHelpers.Cast)dm.CreateDelegate(typeof(TypedReferenceHelpers.Cast));
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000B460 File Offset: 0x00009660
		private unsafe static TypedReferenceHelpers.Make BuildMakeHelper(Type targetType)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
			{
				typeof(void*),
				typeof(TypedRefPtr)
			}, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldarga, 1);
			gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Mkrefany, targetType);
			gen.Emit(OpCodes.Stobj, typeof(TypedReference));
			gen.Emit(OpCodes.Ret);
			return (TypedReferenceHelpers.Make)dm.CreateDelegate(typeof(TypedReferenceHelpers.Make));
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000B524 File Offset: 0x00009724
		private static TypedReferenceHelpers.Unbox BuildUnboxHelper(Type boxType)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
			{
				typeof(object),
				typeof(TypedRefPtr)
			}, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldarga, 1);
			gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Unbox, boxType);
			gen.Emit(OpCodes.Mkrefany, boxType);
			gen.Emit(OpCodes.Stobj, typeof(TypedReference));
			gen.Emit(OpCodes.Ret);
			return (TypedReferenceHelpers.Unbox)dm.CreateDelegate(typeof(TypedReferenceHelpers.Unbox));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B5F4 File Offset: 0x000097F4
		private static TypedReferenceHelpers.Set BuildSetHelper(Type refType)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
			{
				typeof(object),
				typeof(TypedRefPtr)
			}, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			gen.Emit(OpCodes.Ldarga, 1);
			gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
			gen.Emit(OpCodes.Ldobj, typeof(TypedReference));
			gen.Emit(OpCodes.Refanyval, refType);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Unbox_Any, refType);
			gen.Emit(OpCodes.Stobj, refType);
			gen.Emit(OpCodes.Ret);
			return (TypedReferenceHelpers.Set)dm.CreateDelegate(typeof(TypedReferenceHelpers.Set));
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B6D0 File Offset: 0x000098D0
		private static TypedReferenceHelpers.FieldAdr BuildAddrHelper(FieldInfo field)
		{
			DynamicMethod dm = new DynamicMethod("", typeof(void), new Type[]
			{
				typeof(TypedRefPtr),
				typeof(TypedRefPtr)
			}, Unverifier.Module, true);
			ILGenerator gen = dm.GetILGenerator();
			bool isStatic = field.IsStatic;
			if (isStatic)
			{
				gen.Emit(OpCodes.Ldarga, 1);
				gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
				gen.Emit(OpCodes.Ldsflda, field);
				gen.Emit(OpCodes.Mkrefany, field.FieldType);
				gen.Emit(OpCodes.Stobj, typeof(TypedReference));
				gen.Emit(OpCodes.Ret);
			}
			else
			{
				gen.Emit(OpCodes.Ldarga, 1);
				gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
				gen.Emit(OpCodes.Ldarga, 0);
				gen.Emit(OpCodes.Ldfld, TypedReferenceHelpers.typedPtrField);
				gen.Emit(OpCodes.Ldobj, typeof(TypedReference));
				gen.Emit(OpCodes.Refanyval, field.DeclaringType);
				bool flag = !field.DeclaringType.IsValueType;
				if (flag)
				{
					gen.Emit(OpCodes.Ldobj, field.DeclaringType);
				}
				gen.Emit(OpCodes.Ldflda, field);
				gen.Emit(OpCodes.Mkrefany, field.FieldType);
				gen.Emit(OpCodes.Stobj, typeof(TypedReference));
				gen.Emit(OpCodes.Ret);
			}
			return (TypedReferenceHelpers.FieldAdr)dm.CreateDelegate(typeof(TypedReferenceHelpers.FieldAdr));
		}

		// Token: 0x04000046 RID: 70
		private static Hashtable castHelpers = new Hashtable();

		// Token: 0x04000047 RID: 71
		private static Hashtable makeHelpers = new Hashtable();

		// Token: 0x04000048 RID: 72
		private static Hashtable unboxHelpers = new Hashtable();

		// Token: 0x04000049 RID: 73
		private static Hashtable setHelpers = new Hashtable();

		// Token: 0x0400004A RID: 74
		private static Hashtable fieldAddrHelpers = new Hashtable();

		// Token: 0x0400004B RID: 75
		private static FieldInfo typedPtrField = typeof(TypedRefPtr).GetFields()[0];

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x060001BE RID: 446
		private delegate void Cast(TypedRefPtr typedRef);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x060001C2 RID: 450
		private unsafe delegate void Make(void* ptr, TypedRefPtr typedRef);

		// Token: 0x02000083 RID: 131
		// (Invoke) Token: 0x060001C6 RID: 454
		private delegate void Unbox(object box, TypedRefPtr typedRef);

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x060001CA RID: 458
		private delegate void Set(object value, TypedRefPtr typedRef);

		// Token: 0x02000085 RID: 133
		// (Invoke) Token: 0x060001CE RID: 462
		private delegate void FieldAdr(TypedRefPtr value, TypedRefPtr typedRef);
	}
}
