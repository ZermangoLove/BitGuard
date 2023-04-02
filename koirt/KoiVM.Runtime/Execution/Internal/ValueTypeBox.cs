using System;

namespace KoiVM.Runtime.Execution.Internal
{
	// Token: 0x0200006C RID: 108
	internal struct ValueTypeBox<T> : IValueTypeBox
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0000A472 File Offset: 0x00008672
		public ValueTypeBox(T value)
		{
			this.value = value;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A47C File Offset: 0x0000867C
		public object GetValue()
		{
			return this.value;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A49C File Offset: 0x0000869C
		public Type GetValueType()
		{
			return typeof(T);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A4B8 File Offset: 0x000086B8
		public IValueTypeBox Clone()
		{
			return new ValueTypeBox<T>(this.value);
		}

		// Token: 0x0400003D RID: 61
		private T value;
	}
}
