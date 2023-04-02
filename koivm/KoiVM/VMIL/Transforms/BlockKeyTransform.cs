using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.AST.IL;
using KoiVM.CFG;
using KoiVM.RT;
using KoiVM.VM;

namespace KoiVM.VMIL.Transforms
{
	// Token: 0x020000DC RID: 220
	public class BlockKeyTransform : IPostTransform
	{
		// Token: 0x06000348 RID: 840 RVA: 0x00002C93 File Offset: 0x00000E93
		public void Initialize(ILPostTransformer tr)
		{
			this.runtime = tr.Runtime;
			this.methodInfo = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method);
			this.ComputeBlockKeys(tr.RootScope);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001160C File Offset: 0x0000F80C
		private void ComputeBlockKeys(ScopeBlock rootScope)
		{
			List<ILBlock> blocks = rootScope.GetBasicBlocks().OfType<ILBlock>().ToList<ILBlock>();
			uint id = 1U;
			this.Keys = blocks.ToDictionary((ILBlock block) => block, delegate(ILBlock block)
			{
				BlockKeyTransform.BlockKey blockKey = default(BlockKeyTransform.BlockKey);
				uint num = id;
				id = num + 1U;
				blockKey.Entry = num;
				num = id;
				id = num + 1U;
				blockKey.Exit = num;
				return blockKey;
			});
			BlockKeyTransform.EHMap ehMap = this.MapEHs(rootScope);
			Func<BasicBlock<ILInstrList>, uint> <>9__2;
			Func<BasicBlock<ILInstrList>, uint> <>9__3;
			bool updated;
			do
			{
				updated = false;
				BlockKeyTransform.BlockKey key = this.Keys[blocks[0]];
				key.Entry = 4294967294U;
				this.Keys[blocks[0]] = key;
				key = this.Keys[blocks[blocks.Count - 1]];
				key.Exit = 4294967293U;
				this.Keys[blocks[blocks.Count - 1]] = key;
				foreach (ILBlock block3 in blocks)
				{
					key = this.Keys[block3];
					bool flag = block3.Sources.Count > 0;
					if (flag)
					{
						IEnumerable<BasicBlock<ILInstrList>> sources = block3.Sources;
						Func<BasicBlock<ILInstrList>, uint> func;
						if ((func = <>9__2) == null)
						{
							func = (<>9__2 = (BasicBlock<ILInstrList> b) => this.Keys[(ILBlock)b].Exit);
						}
						uint newEntry = sources.Select(func).Max<uint>();
						bool flag2 = key.Entry != newEntry;
						if (flag2)
						{
							key.Entry = newEntry;
							updated = true;
						}
					}
					bool flag3 = block3.Targets.Count > 0;
					if (flag3)
					{
						IEnumerable<BasicBlock<ILInstrList>> targets = block3.Targets;
						Func<BasicBlock<ILInstrList>, uint> func2;
						if ((func2 = <>9__3) == null)
						{
							func2 = (<>9__3 = (BasicBlock<ILInstrList> b) => this.Keys[(ILBlock)b].Entry);
						}
						uint newExit = targets.Select(func2).Max<uint>();
						bool flag4 = key.Exit != newExit;
						if (flag4)
						{
							key.Exit = newExit;
							updated = true;
						}
					}
					this.Keys[block3] = key;
				}
				this.MatchHandlers(ehMap, ref updated);
			}
			while (updated);
			Dictionary<uint, uint> idMap = new Dictionary<uint, uint>();
			idMap[uint.MaxValue] = 0U;
			idMap[4294967294U] = (uint)this.methodInfo.EntryKey;
			idMap[4294967293U] = (uint)this.methodInfo.ExitKey;
			foreach (ILBlock block2 in blocks)
			{
				BlockKeyTransform.BlockKey key2 = this.Keys[block2];
				uint entryId = key2.Entry;
				bool flag5 = !idMap.TryGetValue(entryId, out key2.Entry);
				if (flag5)
				{
					key2.Entry = (idMap[entryId] = (uint)((byte)this.runtime.Descriptor.Random.Next()));
				}
				uint exitId = key2.Exit;
				bool flag6 = !idMap.TryGetValue(exitId, out key2.Exit);
				if (flag6)
				{
					key2.Exit = (idMap[exitId] = (uint)((byte)this.runtime.Descriptor.Random.Next()));
				}
				this.Keys[block2] = key2;
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00011998 File Offset: 0x0000FB98
		private BlockKeyTransform.EHMap MapEHs(ScopeBlock rootScope)
		{
			BlockKeyTransform.EHMap map = new BlockKeyTransform.EHMap();
			this.MapEHsInternal(rootScope, map);
			return map;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000119BC File Offset: 0x0000FBBC
		private void MapEHsInternal(ScopeBlock scope, BlockKeyTransform.EHMap map)
		{
			bool flag = scope.Type == ScopeType.Filter;
			if (flag)
			{
				map.Starts.Add((ILBlock)scope.GetBasicBlocks().First<IBasicBlock>());
			}
			else
			{
				bool flag2 = scope.Type > ScopeType.None;
				if (flag2)
				{
					bool flag3 = scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Finally;
					if (flag3)
					{
						BlockKeyTransform.FinallyInfo info;
						bool flag4 = !map.Finally.TryGetValue(scope.ExceptionHandler, out info);
						if (flag4)
						{
							info = (map.Finally[scope.ExceptionHandler] = new BlockKeyTransform.FinallyInfo());
						}
						bool flag5 = scope.Type == ScopeType.Try;
						if (flag5)
						{
							HashSet<IBasicBlock> scopeBlocks = new HashSet<IBasicBlock>(scope.GetBasicBlocks());
							Func<BasicBlock<ILInstrList>, bool> <>9__0;
							foreach (IBasicBlock basicBlock in scopeBlocks)
							{
								ILBlock block = (ILBlock)basicBlock;
								bool flag6;
								if ((block.Flags & BlockFlags.ExitEHLeave) != BlockFlags.Normal)
								{
									if (block.Targets.Count != 0)
									{
										IEnumerable<BasicBlock<ILInstrList>> targets = block.Targets;
										Func<BasicBlock<ILInstrList>, bool> func;
										if ((func = <>9__0) == null)
										{
											func = (<>9__0 = (BasicBlock<ILInstrList> target) => !scopeBlocks.Contains(target));
										}
										flag6 = targets.Any(func);
									}
									else
									{
										flag6 = true;
									}
								}
								else
								{
									flag6 = false;
								}
								bool flag7 = flag6;
								if (flag7)
								{
									foreach (BasicBlock<ILInstrList> target2 in block.Targets)
									{
										info.TryEndNexts.Add((ILBlock)target2);
									}
								}
							}
						}
						else
						{
							bool flag8 = scope.Type == ScopeType.Handler;
							if (flag8)
							{
								bool flag9 = scope.Children.Count > 0;
								IEnumerable<IBasicBlock> candidates;
								if (flag9)
								{
									candidates = scope.Children.Where((ScopeBlock s) => s.Type == ScopeType.None).SelectMany((ScopeBlock s) => s.GetBasicBlocks());
								}
								else
								{
									candidates = scope.Content;
								}
								foreach (IBasicBlock basicBlock2 in candidates)
								{
									ILBlock block2 = (ILBlock)basicBlock2;
									bool flag10 = (block2.Flags & BlockFlags.ExitEHReturn) != BlockFlags.Normal && block2.Targets.Count == 0;
									if (flag10)
									{
										info.FinallyEnds.Add(block2);
									}
								}
							}
						}
					}
					bool flag11 = scope.Type == ScopeType.Handler;
					if (flag11)
					{
						map.Starts.Add((ILBlock)scope.GetBasicBlocks().First<IBasicBlock>());
					}
				}
			}
			foreach (ScopeBlock child in scope.Children)
			{
				this.MapEHsInternal(child, map);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00011CF0 File Offset: 0x0000FEF0
		private void MatchHandlers(BlockKeyTransform.EHMap map, ref bool updated)
		{
			foreach (ILBlock start in map.Starts)
			{
				BlockKeyTransform.BlockKey key = this.Keys[start];
				bool flag = key.Entry != uint.MaxValue;
				if (flag)
				{
					key.Entry = uint.MaxValue;
					this.Keys[start] = key;
					updated = true;
				}
			}
			foreach (BlockKeyTransform.FinallyInfo info in map.Finally.Values)
			{
				uint maxEnd = info.FinallyEnds.Max((ILBlock block) => this.Keys[block].Exit);
				uint maxEntry = info.TryEndNexts.Max((ILBlock block) => this.Keys[block].Entry);
				uint maxId = Math.Max(maxEnd, maxEntry);
				foreach (ILBlock block3 in info.FinallyEnds)
				{
					BlockKeyTransform.BlockKey key2 = this.Keys[block3];
					bool flag2 = key2.Exit != maxId;
					if (flag2)
					{
						key2.Exit = maxId;
						this.Keys[block3] = key2;
						updated = true;
					}
				}
				foreach (ILBlock block2 in info.TryEndNexts)
				{
					BlockKeyTransform.BlockKey key3 = this.Keys[block2];
					bool flag3 = key3.Entry != maxId;
					if (flag3)
					{
						key3.Entry = maxId;
						this.Keys[block2] = key3;
						updated = true;
					}
				}
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00011F3C File Offset: 0x0001013C
		public void Transform(ILPostTransformer tr)
		{
			BlockKeyTransform.BlockKey key = this.Keys[tr.Block];
			this.methodInfo.BlockKeys[tr.Block] = new VMBlockKey
			{
				EntryKey = (byte)key.Entry,
				ExitKey = (byte)key.Exit
			};
		}

		// Token: 0x04000153 RID: 339
		private VMRuntime runtime;

		// Token: 0x04000154 RID: 340
		private VMMethodInfo methodInfo;

		// Token: 0x04000155 RID: 341
		private Dictionary<ILBlock, BlockKeyTransform.BlockKey> Keys;

		// Token: 0x020000DD RID: 221
		private struct BlockKey
		{
			// Token: 0x04000156 RID: 342
			public uint Entry;

			// Token: 0x04000157 RID: 343
			public uint Exit;
		}

		// Token: 0x020000DE RID: 222
		private class FinallyInfo
		{
			// Token: 0x04000158 RID: 344
			public HashSet<ILBlock> FinallyEnds = new HashSet<ILBlock>();

			// Token: 0x04000159 RID: 345
			public HashSet<ILBlock> TryEndNexts = new HashSet<ILBlock>();
		}

		// Token: 0x020000DF RID: 223
		private class EHMap
		{
			// Token: 0x0400015A RID: 346
			public HashSet<ILBlock> Starts = new HashSet<ILBlock>();

			// Token: 0x0400015B RID: 347
			public Dictionary<ExceptionHandler, BlockKeyTransform.FinallyInfo> Finally = new Dictionary<ExceptionHandler, BlockKeyTransform.FinallyInfo>();
		}
	}
}
