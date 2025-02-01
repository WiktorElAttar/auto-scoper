namespace AutoScopper.Tests.Unit;

public class AutoScopeSnapshotTests
{
    [Fact]
    public Task ShouldGenerate_WhenClassMarkedWithAttribute_WithImportedNamespace()
    {
        var source = """
                     using AutoScoper;

                     [AutoScope]
                     public partial class Test
                     {
                     }
                     """;

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task ShouldGenerate_WhenClassIsMarkedWithAttribute_UsingFullName()
    {
        var source = """
                     [AutoScoper.AutoScope]
                     public partial class Test
                     {
                     }
                     """;

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task ShouldNotGenerate_WhenClassIsMarkedWithAttribute_WithoutNamespaceImport()
    {
        var source = """
                     [AutoScope]
                     public partial class Test
                     {
                     }
                     """;

        return TestHelper.Verify(source);
    }
}
