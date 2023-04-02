using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.AST;
using KoiVM.AST.ILAST;
using KoiVM.AST.IR;

namespace KoiVM.VMIR
{
	// Token: 0x02000022 RID: 34
	public class IRContext
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00002518 File Offset: 0x00000718
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00002520 File Offset: 0x00000720
		public MethodDef Method { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00002529 File Offset: 0x00000729
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00002531 File Offset: 0x00000731
		public bool IsRuntime { get; set; }

		// Token: 0x060000C0 RID: 192 RVA: 0x0000627C File Offset: 0x0000447C
		public IRContext(MethodDef method, CilBody body)
		{
			this.Method = method;
			this.IsRuntime = false;
			this.locals = new IRVariable[body.Variables.Count];
			for (int i = 0; i < this.locals.Length; i++)
			{
				bool isPinned = body.Variables[i].Type.IsPinned;
				if (isPinned)
				{
					throw new NotSupportedException("Pinned variables are not supported.");
				}
				this.locals[i] = new IRVariable
				{
					Id = i,
					Name = "local_" + i.ToString(),
					Type = TypeInference.ToASTType(body.Variables[i].Type),
					RawType = body.Variables[i].Type,
					VariableType = IRVariableType.Local
				};
			}
			this.args = new IRVariable[method.Parameters.Count];
			for (int j = 0; j < this.args.Length; j++)
			{
				this.args[j] = new IRVariable
				{
					Id = j,
					Name = "arg_" + j.ToString(),
					Type = TypeInference.ToASTType(method.Parameters[j].Type),
					RawType = method.Parameters[j].Type,
					VariableType = IRVariableType.Argument
				};
			}
			this.ehVars = new Dictionary<ExceptionHandler, IRVariable>();
			int id = -1;
			foreach (ExceptionHandler eh in body.ExceptionHandlers)
			{
				id++;
				bool flag = eh.HandlerType == ExceptionHandlerType.Fault || eh.HandlerType == ExceptionHandlerType.Finally;
				if (!flag)
				{
					TypeSig type = eh.CatchType.ToTypeSig(true);
					this.ehVars.Add(eh, new IRVariable
					{
						Id = id,
						Name = "ex_" + id.ToString(),
						Type = TypeInference.ToASTType(type),
						RawType = type,
						VariableType = IRVariableType.VirtualRegister
					});
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000064FC File Offset: 0x000046FC
		public IRVariable AllocateVRegister(ASTType type)
		{
			IRVariable vReg = new IRVariable
			{
				Id = this.vRegs.Count,
				Name = "vreg_" + this.vRegs.Count.ToString(),
				Type = type,
				VariableType = IRVariableType.VirtualRegister
			};
			this.vRegs.Add(vReg);
			return vReg;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006568 File Offset: 0x00004768
		public IRVariable ResolveVRegister(ILASTVariable variable)
		{
			bool flag = variable.VariableType == ILASTVariableType.ExceptionVar;
			IRVariable irvariable;
			if (flag)
			{
				irvariable = this.ResolveExceptionVar((ExceptionHandler)variable.Annotation);
			}
			else
			{
				IRVariable vReg;
				bool flag2 = this.varMap.TryGetValue(variable, out vReg);
				if (flag2)
				{
					irvariable = vReg;
				}
				else
				{
					vReg = this.AllocateVRegister(variable.Type);
					this.varMap[variable] = vReg;
					irvariable = vReg;
				}
			}
			return irvariable;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000065D0 File Offset: 0x000047D0
		public IRVariable ResolveParameter(Parameter param)
		{
			return this.args[param.Index];
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000065F0 File Offset: 0x000047F0
		public IRVariable ResolveLocal(Local local)
		{
			return this.locals[local.Index];
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006610 File Offset: 0x00004810
		public IRVariable[] GetParameters()
		{
			return this.args;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006628 File Offset: 0x00004828
		public IRVariable ResolveExceptionVar(ExceptionHandler eh)
		{
			return this.ehVars[eh];
		}

		// Token: 0x04000083 RID: 131
		private List<IRVariable> vRegs = new List<IRVariable>();

		// Token: 0x04000084 RID: 132
		private Dictionary<ILASTVariable, IRVariable> varMap = new Dictionary<ILASTVariable, IRVariable>();

		// Token: 0x04000085 RID: 133
		private IRVariable[] locals;

		// Token: 0x04000086 RID: 134
		private IRVariable[] args;

		// Token: 0x04000087 RID: 135
		private Dictionary<ExceptionHandler, IRVariable> ehVars;
	}
}
