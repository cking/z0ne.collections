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

    public IEnumerable<IFlatTreeBranch<T>> AllBranches => YieldAllBranches();

    public IFlatTreeBranch<T>? Parent => null;

    public IEnumerable<IFlatTreeBranch<T>> Children =>
        AllBranches.Where(branch => branch is FlatTreeBranch<T> && (branch.Parent?.Equals(this) ?? false));

    public IEnumerable<IFlatTreeBranch<T>> Descendents => AllBranches.Skip(count: 1);

    public T Data => nodes[key: 0].Data;

    public IFlatTreeBranch<T> Root => this[index: 0];

    public FlatTree(T root)
    {
        nodes.Add(key: 0, new FlatTreeNode<T>(root, Parent: 0, Depth: 0));
    }

    internal IFlatTreeBranch<T> this[int index] => new FlatTreeBranch<T>(this, index, nodes[index]);

    public static bool operator ==(FlatTree<T>? left, FlatTree<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FlatTree<T>? left, FlatTree<T>? right)
    {
        return !Equals(left, right);
    }

    public static bool operator ==(FlatTree<T> left, IFlatTreeBranch<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FlatTree<T> left, IFlatTreeBranch<T> right)
    {
        return !(left == right);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new[] { nodes[key: 0].Data }.Concat(this[index: 0].Descendents.Select(descendent => descendent.Data))
                                           .GetEnumerator();
    }

    public IFlatTreeBranch<T> AddBranch(T data)
    {
        return Add(data, parent: 0, depth: 1);
    }

    public void RemoveBranch(FlatTreeBranch<T> branch)
    {
        RemoveRecursive(branch);
    }

    public override int GetHashCode()
    {
        return this[index: 0].GetHashCode();
    }

    public bool Equals(IFlatTreeBranch<T>? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is FlatTree<T> other && Equals(other));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal IFlatTreeBranch<T> Add(T data, int parent, int depth)
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
            foreach (var descendent in branch.Descendents.Where(d => d is FlatTreeBranch<T>).Cast<FlatTreeBranch<T>>())
            {
                deletedNodes.Add(descendent.Index);
            }

            deletedNodes.Add(branch.Index);
        }
    }

    private IEnumerable<IFlatTreeBranch<T>> YieldAllBranches()
    {
        yield return this;

        foreach ((var index, var node) in nodes.Skip(count: 1))
        {
            yield return new FlatTreeBranch<T>(this, index, node);
        }
    }
}
