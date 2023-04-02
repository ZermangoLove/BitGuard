using System;
using System.Text;

namespace KoiVM.AST
{
	// Token: 0x02000127 RID: 295
	public class ASTConstant : ASTExpression
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00003771 File Offset: 0x00001971
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00003779 File Offset: 0x00001979
		public object Value { get; set; }

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C580 File Offset: 0x0001A780
		public static void EscapeString(StringBuilder sb, string s, bool addQuotes)
		{
			bool flag = s == null;
			if (flag)
			{
				sb.Append("null");
			}
			else
			{
				if (addQuotes)
				{
					sb.Append('"');
				}
				foreach (char c in s)
				{
					bool flag2 = c < ' ';
					if (flag2)
					{
						switch (c)
						{
						case '\a':
							sb.Append("\\a");
							break;
						case '\b':
							sb.Append("\\b");
							break;
						case '\t':
							sb.Append("\\t");
							break;
						case '\n':
							sb.Append("\\n");
							break;
						case '\v':
							sb.Append("\\v");
							break;
						case '\f':
							sb.Append("\\f");
							break;
						case '\r':
							sb.Append("\\r");
							break;
						default:
							sb.Append(string.Format("\\u{0:X4}", (int)c));
							break;
						}
					}
					else
					{
						bool flag3 = c == '\\' || c == '"';
						if (flag3)
						{
							sb.Append('\\');
							sb.Append(c);
						}
						else
						{
							sb.Append(c);
						}
					}
				}
				if (addQuotes)
				{
					sb.Append('"');
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C6E0 File Offset: 0x0001A8E0
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			bool flag = this.Value == null;
			if (flag)
			{
				ret.Append("<<<NULL>>>");
			}
			else
			{
				bool flag2 = this.Value is string;
				if (flag2)
				{
					ASTConstant.EscapeString(ret, (string)this.Value, true);
				}
				else
				{
					ret.Append(this.Value);
				}
			}
			return ret.ToString();
		}
	}
}
