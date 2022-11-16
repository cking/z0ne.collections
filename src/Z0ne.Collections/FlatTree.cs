// FlatTree.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

using System.Collections;

namespace Z0ne.Collections;

public class FlatTree<T> : IReadOnlyCollection<T>, IFlatTreeBranch<T>
{
    private readonly SortedList<int, FlatTreeNode<T>> nodes = new();

    private readonly HashSet<int> deletedNodes = new();

    private readonly object syncLock = new();

    public int Count
    {
        get
        {
            lock (syncLock)
            {
                return nodes.Count - deletedNodes.Count;
            }
        }
    }

    public IEnumerable<FlatTreeBranch<T>> AllBranches => YieldAllBranches();

    public FlatTreeBranch<T>? Parent => null;

    public IEnumerable<FlatTreeBranch<T>> Children => AllBranches.Where(branch => branch.Parent.Index == 0);

    public IEnumerable<FlatTreeBranch<T>> Descendents => AllBranches.Skip(count: 1);

    public T Data => nodes[key: 0].Data;

    public FlatTreeBranch<T> Root => this[index: 0];

    public FlatTree(T root)
    {
        nodes.Add(key: 0, new FlatTreeNode<T>(root, Parent: 0, Depth: 0));
    }

    internal FlatTreeBranch<T> this[int index] => new(this, index, nodes[index]);

    public IEnumerator<T> GetEnumerator()
    {
        return new[] { nodes[key: 0].Data }.Concat(this[index: 0].Descendents.Select(descendent => descendent.Data))
                                           .GetEnumerator();
    }

    public FlatTreeBranch<T> Add(T data, int parent, int depth)
    {
        lock (syncLock)
        {
            var index = nodes.Count;
            if (deletedNodes.Any())
            {
                index = deletedNodes.First();
                deletedNodes.Remove(index);
            }

            var node = new FlatTreeNode<T>(data, parent, depth);
            nodes.Add(index, node);

            return new FlatTreeBranch<T>(this, index, node);
        }
    }

    public FlatTreeBranch<T> AddBranch(T branch)
    {
        return Add(branch, parent: 0, depth: 1);
    }

    public void RemoveBranch(FlatTreeBranch<T> branch)
    {
        RemoveRecursive(branch);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal bool IsNodeDeleted(int index)
    {
        lock (syncLock)
        {
            return deletedNodes.Contains(index);
        }
    }

    internal void RemoveRecursive(FlatTreeBranch<T> branch)
    {
        lock (syncLock)
        {
            foreach (var descendent in branch.Descendents)
            {
                deletedNodes.Add(descendent.Index);
            }

            deletedNodes.Add(branch.Index);
        }
    }

    private IEnumerable<FlatTreeBranch<T>> YieldAllBranches()
    {
        foreach ((var index, var node) in nodes)
        {
            yield return new FlatTreeBranch<T>(this, index, node);
        }
    }
}
