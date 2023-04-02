using System;
using System.Runtime.CompilerServices;
using System.Text;
using KoiVM.Runtime.Data;
using KoiVM.Runtime.Dynamic;
using KoiVM.Runtime.Execution.Internal;

namespace KoiVM.Runtime.Execution
{
	// Token: 0x02000067 RID: 103
	internal static class VMDispatcher
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00008A64 File Offset: 0x00006C64
		public static ExecutionState Run(VMContext ctx)
		{
			ExecutionState state = ExecutionState.Next;
			bool isAbnormal = true;
			do
			{
				try
				{
					state = VMDispatcher.RunInternal(ctx);
					ExecutionState executionState = state;
					ExecutionState executionState2 = executionState;
					if (executionState2 != ExecutionState.Throw)
					{
						if (executionState2 == ExecutionState.Rethrow)
						{
							uint sp = ctx.Registers[(int)Constants.REG_SP].U4;
							VMSlot ex = ctx.Stack[sp--];
							ctx.Registers[(int)Constants.REG_SP].U4 = sp;
							VMDispatcher.HandleRethrow(ctx, ex.O);
							return state;
						}
					}
					else
					{
						uint sp2 = ctx.Registers[(int)Constants.REG_SP].U4;
						VMSlot ex2 = ctx.Stack[sp2--];
						ctx.Registers[(int)Constants.REG_SP].U4 = sp2;
						VMDispatcher.DoThrow(ctx, ex2.O);
					}
					isAbnormal = false;
				}
				catch (Exception ex3)
				{
					VMDispatcher.SetupEHState(ctx, ex3);
					isAbnormal = false;
				}
				finally
				{
					bool flag = isAbnormal;
					if (flag)
					{
						VMDispatcher.HandleAbnormalExit(ctx);
						state = ExecutionState.Exit;
					}
					else
					{
						bool flag2 = ctx.EHStates.Count > 0;
						if (flag2)
						{
							do
							{
								VMDispatcher.HandleEH(ctx, ref state);
							}
							while (state == ExecutionState.Rethrow);
						}
					}
				}
			}
			while (state != ExecutionState.Exit);
			return state;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008BCC File Offset: 0x00006DCC
		private static Exception Throw(object obj)
		{
			return null;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008BE0 File Offset: 0x00006DE0
		private static ExecutionState RunInternal(VMContext ctx)
		{
			ExecutionState state;
			bool flag2;
			do
			{
				byte op = ctx.ReadByte();
				byte p = ctx.ReadByte();
				OpCodeMap.Lookup(op).Run(ctx, out state);
				bool flag = ctx.Registers[(int)Constants.REG_IP].U8 == 1UL;
				if (flag)
				{
					state = ExecutionState.Exit;
				}
				flag2 = state > ExecutionState.Next;
			}
			while (!flag2);
			return state;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008C44 File Offset: 0x00006E44
		private static void SetupEHState(VMContext ctx, object ex)
		{
			bool flag = ctx.EHStates.Count != 0;
			EHState ehState;
			if (flag)
			{
				ehState = ctx.EHStates[ctx.EHStates.Count - 1];
				bool flag2 = ehState.CurrentFrame != null;
				if (flag2)
				{
					bool flag3 = ehState.CurrentProcess == EHState.EHProcess.Searching;
					if (flag3)
					{
						ctx.Registers[(int)Constants.REG_R1].U1 = 0;
					}
					else
					{
						bool flag4 = ehState.CurrentProcess == EHState.EHProcess.Unwinding;
						if (flag4)
						{
							ehState.ExceptionObj = ex;
						}
					}
					return;
				}
			}
			ehState = new EHState
			{
				OldBP = ctx.Registers[(int)Constants.REG_BP],
				OldSP = ctx.Registers[(int)Constants.REG_SP],
				ExceptionObj = ex,
				CurrentProcess = EHState.EHProcess.Searching,
				CurrentFrame = null,
				HandlerFrame = null
			};
			ctx.EHStates.Add(ehState);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008D38 File Offset: 0x00006F38
		private static void HandleRethrow(VMContext ctx, object ex)
		{
			bool flag = ctx.EHStates.Count > 0;
			if (flag)
			{
				VMDispatcher.SetupEHState(ctx, ex);
			}
			else
			{
				VMDispatcher.DoThrow(ctx, ex);
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008D6C File Offset: 0x00006F6C
		private static string GetIP(VMContext ctx)
		{
			uint ip = ctx.Registers[(int)Constants.REG_IP].U8 - ctx.Instance.Data.KoiSection;
			ulong key = (ulong)((new object().GetHashCode() + Environment.TickCount) | 1);
			return (((ulong)ip * key << 32) | (key & 18446744073709551614UL)).ToString("x16");
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008DD8 File Offset: 0x00006FD8
		private static string StackWalk(VMContext ctx)
		{
			uint ip = ctx.Registers[(int)Constants.REG_IP].U8 - ctx.Instance.Data.KoiSection;
			uint bp = ctx.Registers[(int)Constants.REG_BP].U4;
			StringBuilder sb = new StringBuilder();
			do
			{
				VMDispatcher.rand_state = VMDispatcher.rand_state * 1664525U + 1013904223U;
				ulong key = (ulong)(VMDispatcher.rand_state | 1U);
				sb.AppendFormat("|{0:x16}", ((ulong)ip * key << 32) | (key & 18446744073709551614UL));
				bool flag = bp > 1U;
				if (!flag)
				{
					break;
				}
				ip = ctx.Stack[bp - 1U].U8 - ctx.Instance.Data.KoiSection;
				StackRef bpRef = ctx.Stack[bp].O as StackRef;
				bool flag2 = bpRef == null;
				if (flag2)
				{
					break;
				}
				bp = bpRef.StackPos;
			}
			while (bp > 0U);
			return sb.ToString(1, sb.Length - 1);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008EF8 File Offset: 0x000070F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void DoThrow(VMContext ctx, object ex)
		{
			bool flag = ex is Exception;
			if (flag)
			{
				EHHelper.Rethrow((Exception)ex, VMDispatcher.GetIP(ctx));
			}
			throw VMDispatcher.Throw(ex);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008F30 File Offset: 0x00007130
		private static void HandleEH(VMContext ctx, ref ExecutionState state)
		{
			EHState ehState = ctx.EHStates[ctx.EHStates.Count - 1];
			EHState.EHProcess currentProcess = ehState.CurrentProcess;
			EHState.EHProcess ehprocess = currentProcess;
			int? num;
			if (ehprocess == EHState.EHProcess.Searching)
			{
				bool flag = ehState.CurrentFrame != null;
				if (flag)
				{
					bool filterResult = ctx.Registers[(int)Constants.REG_R1].U1 > 0;
					bool flag2 = filterResult;
					if (flag2)
					{
						ehState.CurrentProcess = EHState.EHProcess.Unwinding;
						ehState.HandlerFrame = ehState.CurrentFrame;
						ehState.CurrentFrame = new int?(ctx.EHStack.Count);
						state = ExecutionState.Next;
						goto IL_321;
					}
					ehState.CurrentFrame--;
				}
				else
				{
					ehState.CurrentFrame = new int?(ctx.EHStack.Count - 1);
				}
				Type exType = ehState.ExceptionObj.GetType();
				int num2;
				EHFrame frame;
				for (;;)
				{
					num = ehState.CurrentFrame;
					num2 = 0;
					if (!((num.GetValueOrDefault() >= num2) & (num != null)) || ehState.HandlerFrame != null)
					{
						goto IL_2A1;
					}
					frame = ctx.EHStack[ehState.CurrentFrame.Value];
					bool flag3 = frame.EHType == Constants.EH_FILTER;
					if (flag3)
					{
						break;
					}
					bool flag4 = frame.EHType == Constants.EH_CATCH;
					if (flag4)
					{
						bool flag5 = frame.CatchType.IsAssignableFrom(exType);
						if (flag5)
						{
							goto Block_8;
						}
					}
					ehState.CurrentFrame--;
				}
				uint sp = ehState.OldSP.U4;
				ctx.Stack.SetTopPosition(sp += 1U);
				ctx.Stack[sp] = new VMSlot
				{
					O = ehState.ExceptionObj
				};
				ctx.Registers[(int)Constants.REG_K1].U1 = 0;
				ctx.Registers[(int)Constants.REG_SP].U4 = sp;
				ctx.Registers[(int)Constants.REG_BP] = frame.BP;
				ctx.Registers[(int)Constants.REG_IP].U8 = frame.FilterAddr;
				goto IL_2A1;
				Block_8:
				ehState.CurrentProcess = EHState.EHProcess.Unwinding;
				ehState.HandlerFrame = ehState.CurrentFrame;
				ehState.CurrentFrame = new int?(ctx.EHStack.Count);
				goto IL_321;
				IL_2A1:
				num = ehState.CurrentFrame;
				num2 = -1;
				bool flag6 = ((num.GetValueOrDefault() == num2) & (num != null)) && ehState.HandlerFrame == null;
				if (flag6)
				{
					ctx.EHStates.RemoveAt(ctx.EHStates.Count - 1);
					state = ExecutionState.Rethrow;
					bool flag7 = ctx.EHStates.Count == 0;
					if (flag7)
					{
						VMDispatcher.HandleRethrow(ctx, ehState.ExceptionObj);
					}
				}
				else
				{
					state = ExecutionState.Next;
				}
				return;
			}
			if (ehprocess != EHState.EHProcess.Unwinding)
			{
				throw new ExecutionEngineException();
			}
			IL_321:
			ehState.CurrentFrame--;
			int i;
			for (i = ehState.CurrentFrame.Value; i > ehState.HandlerFrame.Value; i--)
			{
				EHFrame frame2 = ctx.EHStack[i];
				ctx.EHStack.RemoveAt(i);
				bool flag8 = frame2.EHType == Constants.EH_FAULT || frame2.EHType == Constants.EH_FINALLY;
				if (flag8)
				{
					VMDispatcher.SetupFinallyFrame(ctx, frame2);
					break;
				}
			}
			ehState.CurrentFrame = new int?(i);
			num = ehState.CurrentFrame;
			int? handlerFrame = ehState.HandlerFrame;
			bool flag9 = (num.GetValueOrDefault() == handlerFrame.GetValueOrDefault()) & (num != null == (handlerFrame != null));
			if (flag9)
			{
				EHFrame frame3 = ctx.EHStack[ehState.HandlerFrame.Value];
				ctx.EHStack.RemoveAt(ehState.HandlerFrame.Value);
				uint u = frame3.SP.U4;
				frame3.SP.U4 = u + 1U;
				ctx.Stack.SetTopPosition(frame3.SP.U4);
				ctx.Stack[frame3.SP.U4] = new VMSlot
				{
					O = ehState.ExceptionObj
				};
				ctx.Registers[(int)Constants.REG_K1].U1 = 0;
				ctx.Registers[(int)Constants.REG_SP] = frame3.SP;
				ctx.Registers[(int)Constants.REG_BP] = frame3.BP;
				ctx.Registers[(int)Constants.REG_IP].U8 = frame3.HandlerAddr;
				ctx.EHStates.RemoveAt(ctx.EHStates.Count - 1);
			}
			state = ExecutionState.Next;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000946C File Offset: 0x0000766C
		private static void HandleAbnormalExit(VMContext ctx)
		{
			VMSlot oldBP = ctx.Registers[(int)Constants.REG_BP];
			VMSlot oldSP = ctx.Registers[(int)Constants.REG_SP];
			for (int i = ctx.EHStack.Count - 1; i >= 0; i--)
			{
				EHFrame frame = ctx.EHStack[i];
				bool flag = frame.EHType == Constants.EH_FAULT || frame.EHType == Constants.EH_FINALLY;
				if (flag)
				{
					VMDispatcher.SetupFinallyFrame(ctx, frame);
					VMDispatcher.Run(ctx);
				}
			}
			ctx.EHStack.Clear();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000950C File Offset: 0x0000770C
		private static void SetupFinallyFrame(VMContext ctx, EHFrame frame)
		{
			uint u = frame.SP.U4;
			frame.SP.U4 = u + 1U;
			ctx.Registers[(int)Constants.REG_K1].U1 = 0;
			ctx.Registers[(int)Constants.REG_SP] = frame.SP;
			ctx.Registers[(int)Constants.REG_BP] = frame.BP;
			ctx.Registers[(int)Constants.REG_IP].U8 = frame.HandlerAddr;
			ctx.Stack[frame.SP.U4] = new VMSlot
			{
				U8 = 1UL
			};
		}

		// Token: 0x0400002D RID: 45
		private static uint rand_state = (uint)Environment.TickCount;
	}
}
