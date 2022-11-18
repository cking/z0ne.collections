// FlatTreeBranch.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

using System.Collections;

namespace Z0ne.Collections;

public class FlatTreeBranch<T> : IFlatTreeBranch<T>
{
    private readonly FlatTreeStore<T> store;

    public IFlatTreeBranch<T>? Parent => Depth > 0 ? this[store[Index].Parent] : null;

    public IEnumerable<IFlatTreeBranch<T>> Children => All.Where(branch => (branch.Parent?.Index ?? -1) == Index);

    public IEnumerable<IFlatTreeBranch<T>> Descendents => All.Where(branch => branch.HasParent(this));

    public T Data => store[Index].Data;

    public int Depth => store[Index].Depth;

    public int Index { get; }

    public IFlatTreeBranch<T> Root => this[index: 0];

    public int Count => Children.Count();

    private IEnumerable<IFlatTreeBranch<T>> All => store.Keys.Select(index => new FlatTreeBranch<T>(store, index));

    public FlatTreeBranch(FlatTreeStore<T> store, int index)
    {
        this.store = store;
        Index = index;
    }

    internal IFlatTreeBranch<T> this[int index] => new FlatTreeBranch<T>(store, index);

    public IFlatTreeBranch<T> AddBranch(T data)
    {
        return this[store.Append(data, Index, Depth + 1)];
    }

    public IEnumerator<T> GetEnumerator()
    {
        return store.Values.Select(entry => entry.Data).GetEnumerator();
    }

    public bool Equals(IFlatTreeBranch<T>? other)
    {
        return other is not null && other.Index == Index;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
