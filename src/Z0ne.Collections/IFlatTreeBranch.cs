// IFlatTreeBranch.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public interface IFlatTreeBranch<T>
{
    FlatTreeBranch<T>? Parent { get; }

    IEnumerable<FlatTreeBranch<T>> Children { get; }

    IEnumerable<FlatTreeBranch<T>> Descendents { get; }

    T Data { get; }

    public FlatTreeBranch<T> Root { get; }

    FlatTreeBranch<T> AddBranch(T branch);

    void RemoveBranch(FlatTreeBranch<T> branch);
}
