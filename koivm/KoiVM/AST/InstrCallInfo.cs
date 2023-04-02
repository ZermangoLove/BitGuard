using System;
using dnlib.DotNet;
using KoiVM.AST.IR;

namespace KoiVM.AST
{
	// Token: 0x0200012E RID: 302
	public class InstrCallInfo : InstrAnnotation
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x000037FB File Offset: 0x000019FB
		public InstrCallInfo(string name)
			: base(name)
		{
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00003806 File Offset: 0x00001A06
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x0000380E File Offset: 0x00001A0E
		public ITypeDefOrRef ConstrainType { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00003817 File Offset: 0x00001A17
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x0000381F File Offset: 0x00001A1F
		public IMethod Method { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00003828 File Offset: 0x00001A28
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x00003830 File Offset: 0x00001A30
		public IIROperand[] Arguments { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00003839 File Offset: 0x00001A39
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00003841 File Offset: 0x00001A41
		public IIROperand ReturnValue { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000384A File Offset: 0x00001A4A
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00003852 File Offset: 0x00001A52
		public IRRegister ReturnRegister { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0000385B File Offset: 0x00001A5B
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00003863 File Offset: 0x00001A63
		public IRPointer ReturnSlot { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0000386C File Offset: 0x00001A6C
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00003874 File Offset: 0x00001A74
		public bool IsECall { get; set; }

		// Token: 0x06000508 RID: 1288 RVA: 0x0001C9A8 File Offset: 0x0001ABA8
		public override string ToString()
		{
			string text = base.ToString();
			string text2 = " ";
			IMethod method = this.Method;
			return text + text2 + ((method != null) ? method.ToString() : null);
		}
	}
}
