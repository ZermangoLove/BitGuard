using System;
using System.Reflection;

namespace KoiVM.VMIR
{
	// Token: 0x0200002A RID: 42
	[Obfuscation(Exclude = false, ApplyToMembers = false, Feature = "+rename(forceRen=true);")]
	public enum IROpCode
	{
		// Token: 0x0400009B RID: 155
		NOP,
		// Token: 0x0400009C RID: 156
		MOV,
		// Token: 0x0400009D RID: 157
		POP,
		// Token: 0x0400009E RID: 158
		PUSH,
		// Token: 0x0400009F RID: 159
		CALL,
		// Token: 0x040000A0 RID: 160
		RET,
		// Token: 0x040000A1 RID: 161
		NOR,
		// Token: 0x040000A2 RID: 162
		CMP,
		// Token: 0x040000A3 RID: 163
		JZ,
		// Token: 0x040000A4 RID: 164
		JNZ,
		// Token: 0x040000A5 RID: 165
		JMP,
		// Token: 0x040000A6 RID: 166
		SWT,
		// Token: 0x040000A7 RID: 167
		ADD,
		// Token: 0x040000A8 RID: 168
		SUB,
		// Token: 0x040000A9 RID: 169
		MUL,
		// Token: 0x040000AA RID: 170
		DIV,
		// Token: 0x040000AB RID: 171
		REM,
		// Token: 0x040000AC RID: 172
		SHR,
		// Token: 0x040000AD RID: 173
		SHL,
		// Token: 0x040000AE RID: 174
		FCONV,
		// Token: 0x040000AF RID: 175
		ICONV,
		// Token: 0x040000B0 RID: 176
		SX,
		// Token: 0x040000B1 RID: 177
		VCALL,
		// Token: 0x040000B2 RID: 178
		TRY,
		// Token: 0x040000B3 RID: 179
		LEAVE,
		// Token: 0x040000B4 RID: 180
		Max,
		// Token: 0x040000B5 RID: 181
		__NOT,
		// Token: 0x040000B6 RID: 182
		__AND,
		// Token: 0x040000B7 RID: 183
		__OR,
		// Token: 0x040000B8 RID: 184
		__XOR,
		// Token: 0x040000B9 RID: 185
		__GETF,
		// Token: 0x040000BA RID: 186
		__SETF,
		// Token: 0x040000BB RID: 187
		__CALL,
		// Token: 0x040000BC RID: 188
		__CALLVIRT,
		// Token: 0x040000BD RID: 189
		__NEWOBJ,
		// Token: 0x040000BE RID: 190
		__BEGINCALL,
		// Token: 0x040000BF RID: 191
		__ENDCALL,
		// Token: 0x040000C0 RID: 192
		__ENTRY,
		// Token: 0x040000C1 RID: 193
		__EXIT,
		// Token: 0x040000C2 RID: 194
		__LEAVE,
		// Token: 0x040000C3 RID: 195
		__EHRET,
		// Token: 0x040000C4 RID: 196
		__LDOBJ,
		// Token: 0x040000C5 RID: 197
		__STOBJ,
		// Token: 0x040000C6 RID: 198
		__GEN,
		// Token: 0x040000C7 RID: 199
		__KILL,
		// Token: 0x040000C8 RID: 200
		__LEA
	}
}
