using System;

namespace KoiVM.Runtime.Dynamic
{
	// Token: 0x02000074 RID: 116
	internal static class Constants
	{
		// Token: 0x04000051 RID: 81
		public static byte REG_R0;

		// Token: 0x04000052 RID: 82
		public static byte REG_R1;

		// Token: 0x04000053 RID: 83
		public static byte REG_R2;

		// Token: 0x04000054 RID: 84
		public static byte REG_R3;

		// Token: 0x04000055 RID: 85
		public static byte REG_R4;

		// Token: 0x04000056 RID: 86
		public static byte REG_R5;

		// Token: 0x04000057 RID: 87
		public static byte REG_R6;

		// Token: 0x04000058 RID: 88
		public static byte REG_R7;

		// Token: 0x04000059 RID: 89
		public static byte REG_BP;

		// Token: 0x0400005A RID: 90
		public static byte REG_SP;

		// Token: 0x0400005B RID: 91
		public static byte REG_IP;

		// Token: 0x0400005C RID: 92
		public static byte REG_FL;

		// Token: 0x0400005D RID: 93
		public static byte REG_K1;

		// Token: 0x0400005E RID: 94
		public static byte REG_K2;

		// Token: 0x0400005F RID: 95
		public static byte REG_M1;

		// Token: 0x04000060 RID: 96
		public static byte REG_M2;

		// Token: 0x04000061 RID: 97
		public static byte FL_OVERFLOW;

		// Token: 0x04000062 RID: 98
		public static byte FL_CARRY;

		// Token: 0x04000063 RID: 99
		public static byte FL_ZERO;

		// Token: 0x04000064 RID: 100
		public static byte FL_SIGN;

		// Token: 0x04000065 RID: 101
		public static byte FL_UNSIGNED;

		// Token: 0x04000066 RID: 102
		public static byte FL_BEHAV1;

		// Token: 0x04000067 RID: 103
		public static byte FL_BEHAV2;

		// Token: 0x04000068 RID: 104
		public static byte FL_BEHAV3;

		// Token: 0x04000069 RID: 105
		public static byte OP_NOP;

		// Token: 0x0400006A RID: 106
		public static byte OP_LIND_PTR;

		// Token: 0x0400006B RID: 107
		public static byte OP_LIND_OBJECT;

		// Token: 0x0400006C RID: 108
		public static byte OP_LIND_BYTE;

		// Token: 0x0400006D RID: 109
		public static byte OP_LIND_WORD;

		// Token: 0x0400006E RID: 110
		public static byte OP_LIND_DWORD;

		// Token: 0x0400006F RID: 111
		public static byte OP_LIND_QWORD;

		// Token: 0x04000070 RID: 112
		public static byte OP_SIND_PTR;

		// Token: 0x04000071 RID: 113
		public static byte OP_SIND_OBJECT;

		// Token: 0x04000072 RID: 114
		public static byte OP_SIND_BYTE;

		// Token: 0x04000073 RID: 115
		public static byte OP_SIND_WORD;

		// Token: 0x04000074 RID: 116
		public static byte OP_SIND_DWORD;

		// Token: 0x04000075 RID: 117
		public static byte OP_SIND_QWORD;

		// Token: 0x04000076 RID: 118
		public static byte OP_POP;

		// Token: 0x04000077 RID: 119
		public static byte OP_PUSHR_OBJECT;

		// Token: 0x04000078 RID: 120
		public static byte OP_PUSHR_BYTE;

		// Token: 0x04000079 RID: 121
		public static byte OP_PUSHR_WORD;

		// Token: 0x0400007A RID: 122
		public static byte OP_PUSHR_DWORD;

		// Token: 0x0400007B RID: 123
		public static byte OP_PUSHR_QWORD;

		// Token: 0x0400007C RID: 124
		public static byte OP_PUSHI_DWORD;

		// Token: 0x0400007D RID: 125
		public static byte OP_PUSHI_QWORD;

		// Token: 0x0400007E RID: 126
		public static byte OP_SX_BYTE;

		// Token: 0x0400007F RID: 127
		public static byte OP_SX_WORD;

		// Token: 0x04000080 RID: 128
		public static byte OP_SX_DWORD;

		// Token: 0x04000081 RID: 129
		public static byte OP_CALL;

		// Token: 0x04000082 RID: 130
		public static byte OP_RET;

		// Token: 0x04000083 RID: 131
		public static byte OP_NOR_DWORD;

		// Token: 0x04000084 RID: 132
		public static byte OP_NOR_QWORD;

		// Token: 0x04000085 RID: 133
		public static byte OP_CMP;

		// Token: 0x04000086 RID: 134
		public static byte OP_CMP_DWORD;

		// Token: 0x04000087 RID: 135
		public static byte OP_CMP_QWORD;

		// Token: 0x04000088 RID: 136
		public static byte OP_CMP_R32;

		// Token: 0x04000089 RID: 137
		public static byte OP_CMP_R64;

		// Token: 0x0400008A RID: 138
		public static byte OP_JZ;

		// Token: 0x0400008B RID: 139
		public static byte OP_JNZ;

		// Token: 0x0400008C RID: 140
		public static byte OP_JMP;

		// Token: 0x0400008D RID: 141
		public static byte OP_SWT;

		// Token: 0x0400008E RID: 142
		public static byte OP_ADD_DWORD;

		// Token: 0x0400008F RID: 143
		public static byte OP_ADD_QWORD;

		// Token: 0x04000090 RID: 144
		public static byte OP_ADD_R32;

		// Token: 0x04000091 RID: 145
		public static byte OP_ADD_R64;

		// Token: 0x04000092 RID: 146
		public static byte OP_SUB_R32;

		// Token: 0x04000093 RID: 147
		public static byte OP_SUB_R64;

		// Token: 0x04000094 RID: 148
		public static byte OP_MUL_DWORD;

		// Token: 0x04000095 RID: 149
		public static byte OP_MUL_QWORD;

		// Token: 0x04000096 RID: 150
		public static byte OP_MUL_R32;

		// Token: 0x04000097 RID: 151
		public static byte OP_MUL_R64;

		// Token: 0x04000098 RID: 152
		public static byte OP_DIV_DWORD;

		// Token: 0x04000099 RID: 153
		public static byte OP_DIV_QWORD;

		// Token: 0x0400009A RID: 154
		public static byte OP_DIV_R32;

		// Token: 0x0400009B RID: 155
		public static byte OP_DIV_R64;

		// Token: 0x0400009C RID: 156
		public static byte OP_REM_DWORD;

		// Token: 0x0400009D RID: 157
		public static byte OP_REM_QWORD;

		// Token: 0x0400009E RID: 158
		public static byte OP_REM_R32;

		// Token: 0x0400009F RID: 159
		public static byte OP_REM_R64;

		// Token: 0x040000A0 RID: 160
		public static byte OP_SHR_DWORD;

		// Token: 0x040000A1 RID: 161
		public static byte OP_SHR_QWORD;

		// Token: 0x040000A2 RID: 162
		public static byte OP_SHL_DWORD;

		// Token: 0x040000A3 RID: 163
		public static byte OP_SHL_QWORD;

		// Token: 0x040000A4 RID: 164
		public static byte OP_FCONV_R32_R64;

		// Token: 0x040000A5 RID: 165
		public static byte OP_FCONV_R64_R32;

		// Token: 0x040000A6 RID: 166
		public static byte OP_FCONV_R32;

		// Token: 0x040000A7 RID: 167
		public static byte OP_FCONV_R64;

		// Token: 0x040000A8 RID: 168
		public static byte OP_ICONV_PTR;

		// Token: 0x040000A9 RID: 169
		public static byte OP_ICONV_R64;

		// Token: 0x040000AA RID: 170
		public static byte OP_VCALL;

		// Token: 0x040000AB RID: 171
		public static byte OP_TRY;

		// Token: 0x040000AC RID: 172
		public static byte OP_LEAVE;

		// Token: 0x040000AD RID: 173
		public static byte VCALL_EXIT;

		// Token: 0x040000AE RID: 174
		public static byte VCALL_BREAK;

		// Token: 0x040000AF RID: 175
		public static byte VCALL_ECALL;

		// Token: 0x040000B0 RID: 176
		public static byte VCALL_CAST;

		// Token: 0x040000B1 RID: 177
		public static byte VCALL_CKFINITE;

		// Token: 0x040000B2 RID: 178
		public static byte VCALL_CKOVERFLOW;

		// Token: 0x040000B3 RID: 179
		public static byte VCALL_RANGECHK;

		// Token: 0x040000B4 RID: 180
		public static byte VCALL_INITOBJ;

		// Token: 0x040000B5 RID: 181
		public static byte VCALL_LDFLD;

		// Token: 0x040000B6 RID: 182
		public static byte VCALL_LDFTN;

		// Token: 0x040000B7 RID: 183
		public static byte VCALL_TOKEN;

		// Token: 0x040000B8 RID: 184
		public static byte VCALL_THROW;

		// Token: 0x040000B9 RID: 185
		public static byte VCALL_SIZEOF;

		// Token: 0x040000BA RID: 186
		public static byte VCALL_STFLD;

		// Token: 0x040000BB RID: 187
		public static byte VCALL_BOX;

		// Token: 0x040000BC RID: 188
		public static byte VCALL_UNBOX;

		// Token: 0x040000BD RID: 189
		public static byte VCALL_LOCALLOC;

		// Token: 0x040000BE RID: 190
		public static byte HELPER_INIT;

		// Token: 0x040000BF RID: 191
		public static byte ECALL_CALL;

		// Token: 0x040000C0 RID: 192
		public static byte ECALL_CALLVIRT;

		// Token: 0x040000C1 RID: 193
		public static byte ECALL_NEWOBJ;

		// Token: 0x040000C2 RID: 194
		public static byte ECALL_CALLVIRT_CONSTRAINED;

		// Token: 0x040000C3 RID: 195
		public static byte FLAG_INSTANCE;

		// Token: 0x040000C4 RID: 196
		public static byte EH_CATCH;

		// Token: 0x040000C5 RID: 197
		public static byte EH_FILTER;

		// Token: 0x040000C6 RID: 198
		public static byte EH_FAULT;

		// Token: 0x040000C7 RID: 199
		public static byte EH_FINALLY;
	}
}
