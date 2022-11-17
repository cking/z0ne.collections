// IFlatTreeBranch.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public interface IFlatTreeBranch<T> : IEquatable<IFlatTreeBranch<T>>
{
    IFlatTreeBranch<T>? Parent { get; }

    IEnumerable<IFlatTreeBranch<T>> Children { get; }

    IEnumerable<IFlatTreeBranch<T>> Descendents { get; }

    T Data { get; }

    public IFlatTreeBranch<T> Root { get; }

    IFlatTreeBranch<T> AddBranch(T data);
}
