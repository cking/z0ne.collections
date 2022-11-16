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

    public FlatTreeBranch<T> Parent => tree[node.Parent];

    public IEnumerable<FlatTreeBranch<T>> Children => tree.AllBranches.Where(branch => branch.node.Parent == Index);

    public IEnumerable<FlatTreeBranch<T>> Descendents => tree.AllBranches.Where(branch => branch.node.Parent == Index);

    public T Data => node.Data;

    public FlatTreeBranch<T> Root => tree[index: 0];

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

    public FlatTreeBranch<T> AddBranch(T branch)
    {
        SanityCheck();
        return tree.Add(branch, Index, node.Depth + 1);
    }

    public void RemoveBranch(FlatTreeBranch<T> branch)
    {
        SanityCheck();
        tree.RemoveRecursive(branch);
    }

    private void SanityCheck()
    {
        if (tree.IsNodeDeleted(Index))
        {
            throw new KeyNotFoundException("node has been deleted");
        }

        if (tree[Index].GetHashCode() != node.GetHashCode())
        {
            throw new Exception("node has been changed");
        }
    }
}
