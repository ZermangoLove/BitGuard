using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.AST.ILAST;

namespace KoiVM.ILAST.Transformation
{
	// Token: 0x0200011A RID: 282
	public class VariableInlining : ITransformationHandler
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(ILASTTransformer tr)
		{
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001AE2C File Offset: 0x0001902C
		public static ILASTExpression GetExpression(IILASTStatement node)
		{
			bool flag = node is ILASTExpression;
			ILASTExpression ilastexpression;
			if (flag)
			{
				ILASTExpression expr = (ILASTExpression)node;
				bool flag2 = expr.ILCode == Code.Pop && expr.Arguments[0] is ILASTExpression;
				if (flag2)
				{
					expr = (ILASTExpression)expr.Arguments[0];
				}
				ilastexpression = expr;
			}
			else
			{
				bool flag3 = node is ILASTAssignment;
				if (flag3)
				{
					ilastexpression = ((ILASTAssignment)node).Value;
				}
				else
				{
					ilastexpression = null;
				}
			}
			return ilastexpression;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001AEA4 File Offset: 0x000190A4
		public void Transform(ILASTTransformer tr)
		{
			Dictionary<ILASTVariable, int> varUsage = new Dictionary<ILASTVariable, int>();
			for (int i = 0; i < tr.Tree.Count; i++)
			{
				IILASTStatement st = tr.Tree[i];
				ILASTExpression expr = VariableInlining.GetExpression(st);
				bool flag = expr == null;
				if (!flag)
				{
					bool flag2 = st is ILASTExpression && expr.ILCode == Code.Nop;
					if (flag2)
					{
						tr.Tree.RemoveAt(i);
						i--;
					}
					else
					{
						bool flag3 = st is ILASTAssignment;
						if (flag3)
						{
							ILASTAssignment assignment = (ILASTAssignment)st;
							bool flag4 = Array.IndexOf<ILASTVariable>(tr.Tree.StackRemains, assignment.Variable) != -1;
							if (flag4)
							{
								goto IL_117;
							}
							Debug.Assert(assignment.Variable.VariableType == ILASTVariableType.StackVar);
						}
						foreach (IILASTNode arg in expr.Arguments)
						{
							Debug.Assert(arg is ILASTVariable);
							ILASTVariable argVar = (ILASTVariable)arg;
							bool flag5 = argVar.VariableType == ILASTVariableType.StackVar;
							if (flag5)
							{
								varUsage.Increment(argVar);
							}
						}
					}
				}
				IL_117:;
			}
			foreach (ILASTVariable remain in tr.Tree.StackRemains)
			{
				varUsage.Remove(remain);
			}
			HashSet<ILASTVariable> simpleVars = new HashSet<ILASTVariable>(from usage in varUsage
				where usage.Value == 1
				select usage into pair
				select pair.Key);
			bool modified;
			do
			{
				modified = false;
				for (int j = 0; j < tr.Tree.Count - 1; j++)
				{
					ILASTAssignment assignment2 = tr.Tree[j] as ILASTAssignment;
					bool flag6 = assignment2 == null;
					if (!flag6)
					{
						bool flag7 = !simpleVars.Contains(assignment2.Variable);
						if (!flag7)
						{
							ILASTExpression expr2 = VariableInlining.GetExpression(tr.Tree[j + 1]);
							bool flag8 = expr2 == null || expr2.ILCode.ToOpCode().Name.StartsWith("stelem");
							if (!flag8)
							{
								for (int argIndex = 0; argIndex < expr2.Arguments.Length; argIndex++)
								{
									ILASTVariable argVar2 = expr2.Arguments[argIndex] as ILASTVariable;
									bool flag9 = argVar2 == null;
									if (flag9)
									{
										break;
									}
									bool flag10 = argVar2 == assignment2.Variable;
									if (flag10)
									{
										expr2.Arguments[argIndex] = assignment2.Value;
										tr.Tree.RemoveAt(j);
										j--;
										modified = true;
										break;
									}
								}
								bool flag11 = modified;
								if (flag11)
								{
									break;
								}
							}
						}
					}
				}
			}
			while (modified);
		}
	}
}
