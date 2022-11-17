// FlatTreeTests.cs Copyright (c) z0ne.
// All Rights Reserved.
// Licensed under the EUPL 1.2 License.
// See LICENSE the project root for license information.

using FluentAssertions;

namespace Z0ne.Collections.Tests;

public class FlatTreeTests
{
    private readonly FlatTree<string> sut;

    private readonly int count;

    public FlatTreeTests()
    {
        sut = new FlatTree<string>("root");

        for (var parent = 0; parent < 5; parent++)
        {
            var branch = sut.AddBranch($"branch {parent}");
            count++;

            for (var item = 0; item < 100; item++)
            {
                branch.AddBranch($"child {parent}.{item}");
                count++;
            }
        }
    }

    [Fact]
    public void Parent_OnRootNode_ReturnsNull()
    {
        // Act
        var parent = sut.Parent;

        // Assert
        parent.Should().BeNull();
    }

    [Fact]
    public void Parent_OnSubNode_ReturnsRoot()
    {
        // Act
        var parent = sut.Children.First().Parent;

        // Assert
        parent.Should().Be(sut);
    }

    [Fact]
    public void Children_OnRootNode_ShouldCount5()
    {
        // Act
        var children = sut.Children;

        // Assert
        children.Should().HaveCount(5);
    }

    [Fact]
    public void Children_OnSubNode_ShouldCount100()
    {
        // Act
        var children = sut.Children.First().Children;

        // Assert
        children.Should().HaveCount(100);
    }

    [Fact]
    public void Descendents_OnRootNode_Returns505()
    {
        // Act
        var descendents = sut.Descendents;

        // Assert
        descendents.Should().HaveCount(505);
    }

    [Fact]
    public void Descendents_OnSubNode_Returns100()
    {
        // Act
        var descendents = sut.Children.First().Descendents;

        // Assert
        descendents.Should().HaveCount(100);
    }

    /*
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

    public FlatTreeBranch<T> AddBranch(T data)
    {
        SanityCheck();
        return tree.Add(data, Index, node.Depth + 1);
    }

    public void RemoveBranch(FlatTreeBranch<T> branch)
    {
        SanityCheck();
        tree.RemoveRecursive(branch);
    }
*/
}
