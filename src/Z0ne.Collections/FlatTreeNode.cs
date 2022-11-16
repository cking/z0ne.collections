// FlatTreeNode.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

namespace Z0ne.Collections;

public record FlatTreeNode<T>(T Data, int Parent, int Depth);
