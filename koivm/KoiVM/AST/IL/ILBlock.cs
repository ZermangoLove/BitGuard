using System;
using dnlib.DotNet;
using KoiVM.CFG;
using KoiVM.RT;

namespace KoiVM.AST.IL
{
	// Token: 0x0200013D RID: 317
	public class ILBlock : BasicBlock<ILInstrList>
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x00003B76 File Offset: 0x00001D76
		public ILBlock(int id, ILInstrList content)
			: base(id, content)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001CE58 File Offset: 0x0001B058
		public virtual IKoiChunk CreateChunk(VMRuntime rt, MethodDef method)
		{
			return new BasicBlockChunk(rt, method, this);
		}
	}
}
