// FlatTreeBranchExtensions.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

internal static class FlatTreeBranchExtensions
{
    public static bool HasParent<T>(this IFlatTreeBranch<T> branch, FlatTreeBranch<T> parent)
    {
        while (branch.Parent is not null)
        {
            branch = branch.Parent;
            if (branch.Equals(parent))
            {
                return true;
            }
        }

        return false;
    }
}
