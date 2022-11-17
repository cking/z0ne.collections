// FlatTreeBranch.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public class FlatTreeBranch<T> : IFlatTreeBranch<T>
{
    internal readonly int Index;

    private readonly FlatTree<T> tree;

    private readonly FlatTreeNode<T> node;

    public IFlatTreeBranch<T> Parent => node.Parent == 0 ? tree : tree[node.Parent];

    public IEnumerable<IFlatTreeBranch<T>> Children => tree.AllBranches.Where(branch => branch.Parent?.Equals(this) ?? false);

    public IEnumerable<IFlatTreeBranch<T>> Descendents =>
        tree.AllBranches.Where(branch => branch.HasParent(this));

    public T Data => node.Data;

    public IFlatTreeBranch<T> Root => tree;

    internal FlatTreeBranch(FlatTree<T> tree, int index, FlatTreeNode<T> node)
    {
        this.tree = tree;
        Index = index;
        this.node = node;
    }

    public static implicit operator T(FlatTreeBranch<T> self)
    {
        return self.Data;
    }

    public static bool operator ==(FlatTreeBranch<T> left, IFlatTreeBranch<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FlatTreeBranch<T> left, IFlatTreeBranch<T> right)
    {
        return !(left == right);
    }

    public IFlatTreeBranch<T> AddBranch(T data)
    {
        return tree.Add(data, Index, node.Depth + 1);
    }

    public override int GetHashCode()
    {
        return node.GetHashCode();
    }

    public bool Equals(IFlatTreeBranch<T>? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is FlatTree<T> other && Equals(other));
    }
}
