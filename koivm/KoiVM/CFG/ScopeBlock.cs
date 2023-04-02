using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;

namespace KoiVM.CFG
{
	// Token: 0x02000123 RID: 291
	public class ScopeBlock
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x000036DA File Offset: 0x000018DA
		public ScopeBlock()
		{
			this.Type = ScopeType.None;
			this.ExceptionHandler = null;
			this.Children = new List<ScopeBlock>();
			this.Content = new List<IBasicBlock>();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001BF18 File Offset: 0x0001A118
		public ScopeBlock(ScopeType type, ExceptionHandler eh)
		{
			bool flag = type == ScopeType.None;
			if (flag)
			{
				throw new ArgumentException("type");
			}
			this.Type = type;
			this.ExceptionHandler = eh;
			this.Children = new List<ScopeBlock>();
			this.Content = new List<IBasicBlock>();
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0000370C File Offset: 0x0000190C
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00003714 File Offset: 0x00001914
		public ScopeType Type { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0000371D File Offset: 0x0000191D
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00003725 File Offset: 0x00001925
		public ExceptionHandler ExceptionHandler { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0000372E File Offset: 0x0000192E
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00003736 File Offset: 0x00001936
		public IList<ScopeBlock> Children { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0000373F File Offset: 0x0000193F
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00003747 File Offset: 0x00001947
		public IList<IBasicBlock> Content { get; private set; }

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001BF68 File Offset: 0x0001A168
		public IEnumerable<IBasicBlock> GetBasicBlocks()
		{
			this.Validate();
			bool flag = this.Content.Count > 0;
			IEnumerable<IBasicBlock> enumerable;
			if (flag)
			{
				enumerable = this.Content;
			}
			else
			{
				enumerable = this.Children.SelectMany((ScopeBlock child) => child.GetBasicBlocks());
			}
			return enumerable;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		public Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> UpdateBasicBlocks<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc)
		{
			return this.UpdateBasicBlocks<TOld, TNew>(updateFunc, (int id, TNew content) => new BasicBlock<TNew>(id, content));
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001C000 File Offset: 0x0001A200
		public Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> UpdateBasicBlocks<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc, Func<int, TNew, BasicBlock<TNew>> factoryFunc)
		{
			Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> blockMap = new Dictionary<BasicBlock<TOld>, BasicBlock<TNew>>();
			this.UpdateBasicBlocksInternal<TOld, TNew>(updateFunc, blockMap, factoryFunc);
			foreach (KeyValuePair<BasicBlock<TOld>, BasicBlock<TNew>> blockPair in blockMap)
			{
				foreach (BasicBlock<TOld> src in blockPair.Key.Sources)
				{
					blockPair.Value.Sources.Add(blockMap[src]);
				}
				foreach (BasicBlock<TOld> dst in blockPair.Key.Targets)
				{
					blockPair.Value.Targets.Add(blockMap[dst]);
				}
			}
			return blockMap;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001C120 File Offset: 0x0001A320
		private void UpdateBasicBlocksInternal<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc, Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> blockMap, Func<int, TNew, BasicBlock<TNew>> factoryFunc)
		{
			this.Validate();
			bool flag = this.Content.Count > 0;
			if (flag)
			{
				for (int i = 0; i < this.Content.Count; i++)
				{
					BasicBlock<TOld> oldBlock = (BasicBlock<TOld>)this.Content[i];
					TNew newContent = updateFunc(oldBlock);
					BasicBlock<TNew> newBlock = factoryFunc(oldBlock.Id, newContent);
					newBlock.Flags = oldBlock.Flags;
					this.Content[i] = newBlock;
					blockMap[oldBlock] = newBlock;
				}
			}
			else
			{
				foreach (ScopeBlock child in this.Children)
				{
					child.UpdateBasicBlocksInternal<TOld, TNew>(updateFunc, blockMap, factoryFunc);
				}
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001C208 File Offset: 0x0001A408
		public void ProcessBasicBlocks<T>(Action<BasicBlock<T>> processFunc)
		{
			this.Validate();
			bool flag = this.Content.Count > 0;
			if (flag)
			{
				foreach (IBasicBlock child in this.Content)
				{
					processFunc((BasicBlock<T>)child);
				}
			}
			else
			{
				foreach (ScopeBlock child2 in this.Children)
				{
					child2.ProcessBasicBlocks<T>(processFunc);
				}
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
		public void Validate()
		{
			bool flag = this.Children.Count != 0 && this.Content.Count != 0;
			if (flag)
			{
				throw new InvalidOperationException("Children and Content cannot be set at the same time.");
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		public ScopeBlock[] SearchBlock(IBasicBlock target)
		{
			Stack<ScopeBlock> scopeStack = new Stack<ScopeBlock>();
			ScopeBlock.SearchBlockInternal(this, target, scopeStack);
			return scopeStack.Reverse<ScopeBlock>().ToArray<ScopeBlock>();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001C328 File Offset: 0x0001A528
		private static bool SearchBlockInternal(ScopeBlock scope, IBasicBlock target, Stack<ScopeBlock> scopeStack)
		{
			bool flag = scope.Content.Count > 0;
			bool flag3;
			if (flag)
			{
				bool flag2 = scope.Content.Contains(target);
				if (flag2)
				{
					scopeStack.Push(scope);
					flag3 = true;
				}
				else
				{
					flag3 = false;
				}
			}
			else
			{
				scopeStack.Push(scope);
				foreach (ScopeBlock child in scope.Children)
				{
					bool flag4 = ScopeBlock.SearchBlockInternal(child, target, scopeStack);
					if (flag4)
					{
						return true;
					}
				}
				scopeStack.Pop();
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001C3D0 File Offset: 0x0001A5D0
		private static string ToString(ExceptionHandler eh)
		{
			return string.Format("{0:x8}:{1}", eh.GetHashCode(), eh.HandlerType);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001C404 File Offset: 0x0001A604
		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			bool flag = this.Type == ScopeType.Try;
			if (flag)
			{
				ret.AppendLine("try @ " + ScopeBlock.ToString(this.ExceptionHandler) + " {");
			}
			else
			{
				bool flag2 = this.Type == ScopeType.Handler;
				if (flag2)
				{
					ret.AppendLine("handler @ " + ScopeBlock.ToString(this.ExceptionHandler) + " {");
				}
				else
				{
					bool flag3 = this.Type == ScopeType.Filter;
					if (flag3)
					{
						ret.AppendLine("filter @ " + ScopeBlock.ToString(this.ExceptionHandler) + " {");
					}
					else
					{
						ret.AppendLine("{");
					}
				}
			}
			bool flag4 = this.Children.Count > 0;
			if (flag4)
			{
				foreach (ScopeBlock child in this.Children)
				{
					ret.AppendLine(child.ToString());
				}
			}
			else
			{
				foreach (IBasicBlock child2 in this.Content)
				{
					ret.AppendLine(child2.ToString());
				}
			}
			ret.AppendLine("}");
			return ret.ToString();
		}
	}
}
