﻿using System;
using System.Reflection;

namespace KoiVM.VMIL
{
	// Token: 0x020000B3 RID: 179
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum ILOpCode
	{
		// Token: 0x040000FB RID: 251
		NOP,
		// Token: 0x040000FC RID: 252
		LIND_PTR,
		// Token: 0x040000FD RID: 253
		LIND_OBJECT,
		// Token: 0x040000FE RID: 254
		LIND_BYTE,
		// Token: 0x040000FF RID: 255
		LIND_WORD,
		// Token: 0x04000100 RID: 256
		LIND_DWORD,
		// Token: 0x04000101 RID: 257
		LIND_QWORD,
		// Token: 0x04000102 RID: 258
		SIND_PTR,
		// Token: 0x04000103 RID: 259
		SIND_OBJECT,
		// Token: 0x04000104 RID: 260
		SIND_BYTE,
		// Token: 0x04000105 RID: 261
		SIND_WORD,
		// Token: 0x04000106 RID: 262
		SIND_DWORD,
		// Token: 0x04000107 RID: 263
		SIND_QWORD,
		// Token: 0x04000108 RID: 264
		POP,
		// Token: 0x04000109 RID: 265
		PUSHR_OBJECT,
		// Token: 0x0400010A RID: 266
		PUSHR_BYTE,
		// Token: 0x0400010B RID: 267
		PUSHR_WORD,
		// Token: 0x0400010C RID: 268
		PUSHR_DWORD,
		// Token: 0x0400010D RID: 269
		PUSHR_QWORD,
		// Token: 0x0400010E RID: 270
		PUSHI_DWORD,
		// Token: 0x0400010F RID: 271
		PUSHI_QWORD,
		// Token: 0x04000110 RID: 272
		SX_BYTE,
		// Token: 0x04000111 RID: 273
		SX_WORD,
		// Token: 0x04000112 RID: 274
		SX_DWORD,
		// Token: 0x04000113 RID: 275
		CALL,
		// Token: 0x04000114 RID: 276
		RET,
		// Token: 0x04000115 RID: 277
		NOR_DWORD,
		// Token: 0x04000116 RID: 278
		NOR_QWORD,
		// Token: 0x04000117 RID: 279
		CMP,
		// Token: 0x04000118 RID: 280
		CMP_DWORD,
		// Token: 0x04000119 RID: 281
		CMP_QWORD,
		// Token: 0x0400011A RID: 282
		CMP_R32,
		// Token: 0x0400011B RID: 283
		CMP_R64,
		// Token: 0x0400011C RID: 284
		JZ,
		// Token: 0x0400011D RID: 285
		JNZ,
		// Token: 0x0400011E RID: 286
		JMP,
		// Token: 0x0400011F RID: 287
		SWT,
		// Token: 0x04000120 RID: 288
		ADD_DWORD,
		// Token: 0x04000121 RID: 289
		ADD_QWORD,
		// Token: 0x04000122 RID: 290
		ADD_R32,
		// Token: 0x04000123 RID: 291
		ADD_R64,
		// Token: 0x04000124 RID: 292
		SUB_R32,
		// Token: 0x04000125 RID: 293
		SUB_R64,
		// Token: 0x04000126 RID: 294
		MUL_DWORD,
		// Token: 0x04000127 RID: 295
		MUL_QWORD,
		// Token: 0x04000128 RID: 296
		MUL_R32,
		// Token: 0x04000129 RID: 297
		MUL_R64,
		// Token: 0x0400012A RID: 298
		DIV_DWORD,
		// Token: 0x0400012B RID: 299
		DIV_QWORD,
		// Token: 0x0400012C RID: 300
		DIV_R32,
		// Token: 0x0400012D RID: 301
		DIV_R64,
		// Token: 0x0400012E RID: 302
		REM_DWORD,
		// Token: 0x0400012F RID: 303
		REM_QWORD,
		// Token: 0x04000130 RID: 304
		REM_R32,
		// Token: 0x04000131 RID: 305
		REM_R64,
		// Token: 0x04000132 RID: 306
		SHR_DWORD,
		// Token: 0x04000133 RID: 307
		SHR_QWORD,
		// Token: 0x04000134 RID: 308
		SHL_DWORD,
		// Token: 0x04000135 RID: 309
		SHL_QWORD,
		// Token: 0x04000136 RID: 310
		FCONV_R32_R64,
		// Token: 0x04000137 RID: 311
		FCONV_R64_R32,
		// Token: 0x04000138 RID: 312
		FCONV_R32,
		// Token: 0x04000139 RID: 313
		FCONV_R64,
		// Token: 0x0400013A RID: 314
		ICONV_PTR,
		// Token: 0x0400013B RID: 315
		ICONV_R64,
		// Token: 0x0400013C RID: 316
		VCALL,
		// Token: 0x0400013D RID: 317
		TRY,
		// Token: 0x0400013E RID: 318
		LEAVE,
		// Token: 0x0400013F RID: 319
		Max,
		// Token: 0x04000140 RID: 320
		__ENTRY,
		// Token: 0x04000141 RID: 321
		__EXIT,
		// Token: 0x04000142 RID: 322
		__BEGINCALL,
		// Token: 0x04000143 RID: 323
		__ENDCALL
	}
}
