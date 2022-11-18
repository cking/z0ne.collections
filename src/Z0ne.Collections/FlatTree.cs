// FlatTree.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public class FlatTree<T> : FlatTreeBranch<T>
{
    public FlatTree(T root)
        : base(new FlatTreeStore<T>(root), index: 0)
    {
    }
}
