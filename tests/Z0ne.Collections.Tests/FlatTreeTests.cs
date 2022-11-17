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

    [Fact]
    public void Root_OnRootNode_ReturnsItself()
    {
        // Act
        var root = sut.Root;

        // Assert
        root.Should().BeEquivalentTo(sut);
    }

    [Fact]
    public void Root_OnSubNode_ReturnsRoot()
    {
        // Act
        var root = sut.Children.First().Root;

        // Assert
        root.Should().BeEquivalentTo(sut);
    }

    [Fact]
    public void AddBranch_WithData_ShouldAddNewBranch()
    {
        // Act
        var child = sut.AddBranch("data");

        // Assert
        sut.Children.Should().HaveCount(6);
        sut.Children.Last().Should().BeEquivalentTo(child);
    }

}
