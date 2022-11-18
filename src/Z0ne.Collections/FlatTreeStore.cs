// FlatTreeStore.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public class FlatTreeStore<T> : SortedList<int, (T Data, int Parent, int Depth)>
{
    public FlatTreeStore(T data)
    {
        Add(key: 0, (data, 0, 0));
    }

    public int Append(T data, int parent, int depth)
    {
        Add(Count, (data, parent, depth));
        return Count - 1;
    }
}
