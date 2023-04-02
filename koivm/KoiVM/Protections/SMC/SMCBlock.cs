using System;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IL;
using KoiVM.RT;

namespace KoiVM.Protections.SMC
{
	// Token: 0x02000106 RID: 262
	internal class SMCBlock : ILBlock
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x0000333E File Offset: 0x0000153E
		public SMCBlock(int id, ILInstrList content)
			: base(id, content)
		{
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000334A File Offset: 0x0000154A
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00003352 File Offset: 0x00001552
		public byte Key { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000335B File Offset: 0x0000155B
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00003363 File Offset: 0x00001563
		public ILImmediate CounterOperand { get; set; }

		// Token: 0x06000438 RID: 1080 RVA: 0x000184FC File Offset: 0x000166FC
		public override IKoiChunk CreateChunk(VMRuntime rt, MethodDef method)
		{
			return new SMCBlockChunk(rt, method, this);
		}

		// Token: 0x040001D1 RID: 465
		internal static readonly InstrAnnotation CounterInit = new InstrAnnotation("SMC_COUNTER");

		// Token: 0x040001D2 RID: 466
		internal static readonly InstrAnnotation EncryptionKey = new InstrAnnotation("SMC_KEY");

		// Token: 0x040001D3 RID: 467
		internal static readonly InstrAnnotation AddressPart1 = new InstrAnnotation("SMC_PART1");

		// Token: 0x040001D4 RID: 468
		internal static readonly InstrAnnotation AddressPart2 = new InstrAnnotation("SMC_PART2");
	}
}
