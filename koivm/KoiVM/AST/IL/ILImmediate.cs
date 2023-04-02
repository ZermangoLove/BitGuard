using System;

namespace KoiVM.AST.IL
{
	// Token: 0x02000145 RID: 325
	public class ILImmediate : ASTConstant, IILOperand
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0001D0EC File Offset: 0x0001B2EC
		public static ILImmediate Create(object value, ASTType type)
		{
			return new ILImmediate
			{
				Value = value,
				Type = new ASTType?(type)
			};
		}
	}
}
