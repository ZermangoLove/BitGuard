using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IR;
using KoiVM.VM;

namespace KoiVM.VMIR.Transforms
{
	// Token: 0x020000A8 RID: 168
	public class InvokeTransform : ITransform
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000227A File Offset: 0x0000047A
		public void Initialize(IRTransformer tr)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000027B7 File Offset: 0x000009B7
		public void Transform(IRTransformer tr)
		{
			tr.Instructions.VisitInstrs<IRTransformer>(new VisitFunc<IRInstrList, IRInstruction, IRTransformer>(this.VisitInstr), tr);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F04C File Offset: 0x0000D24C
		private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
		{
			bool flag = instr.OpCode != IROpCode.__CALL && instr.OpCode != IROpCode.__CALLVIRT && instr.OpCode != IROpCode.__NEWOBJ;
			if (!flag)
			{
				MethodDef method = ((IMethod)((IRMetaTarget)instr.Operand1).MetadataItem).ResolveMethodDef();
				InstrCallInfo callInfo = (InstrCallInfo)instr.Annotation;
				bool flag2 = method == null || method.Module != tr.Context.Method.Module || !tr.VM.Settings.IsVirtualized(method) || instr.OpCode != IROpCode.__CALL;
				if (flag2)
				{
					callInfo.IsECall = true;
					this.ProcessECall(instrs, instr, index, tr);
				}
				else
				{
					callInfo.IsECall = false;
					this.ProcessDCall(instrs, instr, index, tr, method);
				}
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F124 File Offset: 0x0000D324
		private void ProcessECall(IRInstrList instrs, IRInstruction instr, int index, IRTransformer tr)
		{
			IMethod method = (IMethod)((IRMetaTarget)instr.Operand1).MetadataItem;
			IRVariable retVar = (IRVariable)instr.Operand2;
			uint opCode = 0U;
			ITypeDefOrRef constrainType = ((InstrCallInfo)instr.Annotation).ConstrainType;
			bool flag = instr.OpCode == IROpCode.__CALL;
			if (flag)
			{
				opCode = tr.VM.Runtime.VCallOps.ECALL_CALL;
			}
			else
			{
				bool flag2 = instr.OpCode == IROpCode.__CALLVIRT;
				if (flag2)
				{
					bool flag3 = constrainType != null;
					if (flag3)
					{
						opCode = tr.VM.Runtime.VCallOps.ECALL_CALLVIRT_CONSTRAINED;
					}
					else
					{
						opCode = tr.VM.Runtime.VCallOps.ECALL_CALLVIRT;
					}
				}
				else
				{
					bool flag4 = instr.OpCode == IROpCode.__NEWOBJ;
					if (flag4)
					{
						opCode = tr.VM.Runtime.VCallOps.ECALL_NEWOBJ;
					}
				}
			}
			int methodId = (int)(tr.VM.Data.GetId(method) | (opCode << 30));
			int ecallId = tr.VM.Runtime.VMCall.ECALL;
			List<IRInstruction> callInstrs = new List<IRInstruction>();
			bool flag5 = constrainType != null;
			if (flag5)
			{
				callInstrs.Add(new IRInstruction(IROpCode.PUSH)
				{
					Operand1 = IRConstant.FromI4((int)tr.VM.Data.GetId(constrainType)),
					Annotation = instr.Annotation,
					ILAST = instr.ILAST
				});
			}
			callInstrs.Add(new IRInstruction(IROpCode.VCALL)
			{
				Operand1 = IRConstant.FromI4(ecallId),
				Operand2 = IRConstant.FromI4(methodId),
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
			bool flag6 = retVar != null;
			if (flag6)
			{
				callInstrs.Add(new IRInstruction(IROpCode.POP, retVar)
				{
					Annotation = instr.Annotation,
					ILAST = instr.ILAST
				});
			}
			instrs.Replace(index, callInstrs);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F318 File Offset: 0x0000D518
		private void ProcessDCall(IRInstrList instrs, IRInstruction instr, int index, IRTransformer tr, MethodDef method)
		{
			IRVariable retVar = (IRVariable)instr.Operand2;
			InstrCallInfo callinfo = (InstrCallInfo)instr.Annotation;
			callinfo.Method = method;
			List<IRInstruction> callInstrs = new List<IRInstruction>();
			callInstrs.Add(new IRInstruction(IROpCode.CALL, new IRMetaTarget(method)
			{
				LateResolve = true
			})
			{
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
			bool flag = retVar != null;
			if (flag)
			{
				callInstrs.Add(new IRInstruction(IROpCode.MOV, retVar, new IRRegister(VMRegisters.R0, retVar.Type))
				{
					Annotation = instr.Annotation,
					ILAST = instr.ILAST
				});
			}
			int stackAdjust = -callinfo.Arguments.Length;
			callInstrs.Add(new IRInstruction(IROpCode.ADD, IRRegister.SP, IRConstant.FromI4(stackAdjust))
			{
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
			instrs.Replace(index, callInstrs);
		}
	}
}
