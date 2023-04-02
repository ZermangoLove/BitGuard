using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.RT.Mutation
{
	// Token: 0x020000FA RID: 250
	public class Renamer
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x000031B4 File Offset: 0x000013B4
		public Renamer(int seed)
		{
			this.next = seed;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00016B00 File Offset: 0x00014D00
		private string ToString(int id)
		{
			return id.ToString();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00016B1C File Offset: 0x00014D1C
		private string NewName(string name)
		{
			string text;
			bool flag = !this.nameMap.TryGetValue(name, out text);
			if (flag)
			{
				text = (this.nameMap[name] = new string((from s in Enumerable.Repeat<string>("123242353464356234871284672374712543274512548923150923", 10)
					select s[Renamer.random.Next(s.Length)]).ToArray<char>()));
			}
			return text.ToString();
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00016B94 File Offset: 0x00014D94
		public void Process(ModuleDef module)
		{
			foreach (TypeDef type in module.GetTypes())
			{
				bool flag = !type.IsPublic;
				if (flag)
				{
					type.Namespace = "";
					type.Name = this.NewName(type.FullName);
				}
				foreach (GenericParam genParam in type.GenericParameters)
				{
					genParam.Name = "";
				}
				bool isDelegate = type.BaseType != null && (type.BaseType.FullName == "System.Delegate" || type.BaseType.FullName == "System.MulticastDelegate");
				foreach (MethodDef method in type.Methods)
				{
					bool hasBody = method.HasBody;
					if (hasBody)
					{
						foreach (Instruction instr in method.Body.Instructions)
						{
							MemberRef memberRef = instr.Operand as MemberRef;
							bool flag2 = memberRef != null;
							if (flag2)
							{
								TypeDef typeDef = memberRef.DeclaringType.ResolveTypeDef();
								bool flag3 = memberRef.IsMethodRef && typeDef != null;
								if (flag3)
								{
									MethodDef target = typeDef.ResolveMethod(memberRef);
									bool flag4 = target != null && target.IsRuntimeSpecialName;
									if (flag4)
									{
										typeDef = null;
									}
								}
								bool flag5 = typeDef != null && typeDef.Module == module;
								if (flag5)
								{
									memberRef.Name = this.NewName(memberRef.Name);
								}
							}
						}
					}
					foreach (Parameter arg in ((IEnumerable<Parameter>)method.Parameters))
					{
						arg.Name = "";
					}
					bool flag6 = method.IsRuntimeSpecialName || isDelegate || type.IsPublic;
					if (!flag6)
					{
						method.Name = this.NewName(method.Name);
						method.CustomAttributes.Clear();
					}
				}
				for (int i = 0; i < type.Fields.Count; i++)
				{
					FieldDef field = type.Fields[i];
					bool isLiteral = field.IsLiteral;
					if (isLiteral)
					{
						type.Fields.RemoveAt(i--);
					}
					else
					{
						bool isRuntimeSpecialName = field.IsRuntimeSpecialName;
						if (!isRuntimeSpecialName)
						{
							field.Name = this.NewName(field.Name);
						}
					}
				}
				type.Properties.Clear();
				type.Events.Clear();
				type.CustomAttributes.Clear();
			}
		}

		// Token: 0x040001A0 RID: 416
		private Dictionary<string, string> nameMap = new Dictionary<string, string>();

		// Token: 0x040001A1 RID: 417
		private int next;

		// Token: 0x040001A2 RID: 418
		private static Random random = new Random();
	}
}
