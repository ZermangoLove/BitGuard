using System;
using System.Linq;
using dnlib.DotNet;
using KoiVM.AST;
using KoiVM.AST.IR;
using KoiVM.CFG;
using KoiVM.Protections.SMC;
using KoiVM.VMIR;

namespace KoiVM.Protections
{
	// Token: 0x02000105 RID: 261
	public class SMCIRTransform : ITransform
	{
		// Token: 0x06000430 RID: 1072 RVA: 0x00018108 File Offset: 0x00016308
		public void Initialize(IRTransformer tr)
		{
			bool flag = tr.Context.Method.Module == null;
			if (flag)
			{
				this.doWork = false;
			}
			else
			{
				IRVariable counter = tr.Context.AllocateVRegister(ASTType.I4);
				IRVariable pointer = tr.Context.AllocateVRegister(ASTType.Ptr);
				IRVariable t = tr.Context.AllocateVRegister(ASTType.I4);
				IRVariable t2 = tr.Context.AllocateVRegister(ASTType.I4);
				IRVariable t3 = tr.Context.AllocateVRegister(ASTType.I4);
				BasicBlock<IRInstrList> entry = (BasicBlock<IRInstrList>)tr.RootScope.GetBasicBlocks().First<IBasicBlock>();
				BasicBlock<IRInstrList> entryStub = new BasicBlock<IRInstrList>(-3, new IRInstrList());
				BasicBlock<IRInstrList> loopBody = new BasicBlock<IRInstrList>(-2, new IRInstrList());
				BasicBlock<IRInstrList> trampoline = new BasicBlock<IRInstrList>(-1, new IRInstrList());
				ITypeDefOrRef byteType = tr.Context.Method.Module.CorLibTypes.Byte.ToTypeDefOrRef();
				entryStub.Content.AddRange(new IRInstruction[]
				{
					new IRInstruction(IROpCode.MOV, counter, IRConstant.FromI4(251658241), SMCBlock.CounterInit),
					new IRInstruction(IROpCode.MOV, pointer, new IRBlockTarget(trampoline)),
					new IRInstruction(IROpCode.ADD, pointer, IRConstant.FromI4(-1)),
					new IRInstruction(IROpCode.__LDOBJ, pointer, t, new PointerInfo("LDOBJ", byteType)),
					new IRInstruction(IROpCode.CMP, t, IRConstant.FromI4(0)),
					new IRInstruction(IROpCode.__GETF, t, IRConstant.FromI4(1 << tr.VM.Architecture.Flags.ZERO)),
					new IRInstruction(IROpCode.JNZ, new IRBlockTarget(trampoline), t),
					new IRInstruction(IROpCode.CMP, counter, IRConstant.FromI4(0)),
					new IRInstruction(IROpCode.__GETF, t, IRConstant.FromI4(1 << tr.VM.Architecture.Flags.ZERO)),
					new IRInstruction(IROpCode.JZ, new IRBlockTarget(loopBody), t),
					new IRInstruction(IROpCode.MOV, t3, new IRBlockTarget(entry), SMCBlock.AddressPart1),
					new IRInstruction(IROpCode.JMP, new IRBlockTarget(trampoline))
				});
				entryStub.LinkTo(loopBody);
				entryStub.LinkTo(trampoline);
				loopBody.Content.AddRange(new IRInstruction[]
				{
					new IRInstruction(IROpCode.__LDOBJ, pointer, t2, new PointerInfo("LDOBJ", byteType)),
					new IRInstruction(IROpCode.__XOR, t2, IRConstant.FromI4(251658242), SMCBlock.EncryptionKey),
					new IRInstruction(IROpCode.__STOBJ, pointer, t2, new PointerInfo("STOBJ", byteType)),
					new IRInstruction(IROpCode.ADD, counter, IRConstant.FromI4(-1)),
					new IRInstruction(IROpCode.ADD, pointer, IRConstant.FromI4(1)),
					new IRInstruction(IROpCode.CMP, counter, IRConstant.FromI4(0)),
					new IRInstruction(IROpCode.__GETF, t2, IRConstant.FromI4(1 << tr.VM.Architecture.Flags.ZERO)),
					new IRInstruction(IROpCode.JZ, new IRBlockTarget(loopBody), t2),
					new IRInstruction(IROpCode.MOV, t3, new IRBlockTarget(entry), SMCBlock.AddressPart1),
					new IRInstruction(IROpCode.NOP),
					new IRInstruction(IROpCode.JMP, new IRBlockTarget(trampoline))
				});
				loopBody.LinkTo(loopBody);
				loopBody.LinkTo(trampoline);
				trampoline.Content.AddRange(new IRInstruction[]
				{
					new IRInstruction(IROpCode.NOP),
					new IRInstruction(IROpCode.NOP),
					new IRInstruction(IROpCode.__XOR, t3, IRConstant.FromI4(251658243), SMCBlock.AddressPart2),
					new IRInstruction(IROpCode.JMP, new IRBlockTarget(entry))
				});
				trampoline.LinkTo(entry);
				ScopeBlock scope = tr.RootScope.SearchBlock(entry).Last<ScopeBlock>();
				scope.Content.Insert(0, entryStub);
				scope.Content.Insert(1, loopBody);
				scope.Content.Insert(2, trampoline);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000184D0 File Offset: 0x000166D0
		public void Transform(IRTransformer tr)
		{
			bool flag = this.doWork;
			if (flag)
			{
				tr.Block.Id += 3;
			}
		}

		// Token: 0x040001D0 RID: 464
		private bool doWork = true;
	}
}
