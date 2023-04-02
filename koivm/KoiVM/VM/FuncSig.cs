using System;
using dnlib.DotNet;

namespace KoiVM.VM
{
	// Token: 0x02000013 RID: 19
	public class FuncSig
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00005A84 File Offset: 0x00003C84
		public override int GetHashCode()
		{
			SigComparer comparer = default(SigComparer);
			int hashCode = (int)this.Flags;
			foreach (ITypeDefOrRef param in this.ParamSigs)
			{
				hashCode = hashCode * 7 + comparer.GetHashCode(param);
			}
			return hashCode * 7 + comparer.GetHashCode(this.RetType);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005AE4 File Offset: 0x00003CE4
		public override bool Equals(object obj)
		{
			FuncSig other = obj as FuncSig;
			bool flag = other == null || other.Flags != this.Flags;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = other.ParamSigs.Length != this.ParamSigs.Length;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					SigComparer comparer = default(SigComparer);
					for (int i = 0; i < this.ParamSigs.Length; i++)
					{
						bool flag4 = !comparer.Equals(this.ParamSigs[i], other.ParamSigs[i]);
						if (flag4)
						{
							return false;
						}
					}
					bool flag5 = !comparer.Equals(this.RetType, other.RetType);
					flag2 = !flag5;
				}
			}
			return flag2;
		}

		// Token: 0x0400002F RID: 47
		public byte Flags;

		// Token: 0x04000030 RID: 48
		public ITypeDefOrRef[] ParamSigs;

		// Token: 0x04000031 RID: 49
		public ITypeDefOrRef RetType;
	}
}
