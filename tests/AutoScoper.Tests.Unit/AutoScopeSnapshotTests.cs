namespace AutoScoper.Tests.Unit;

public class AutoScopeSnapshotTests
{
    [Fact]
    public Task ShouldGenerate_WhenClassMarkedWithAttribute_WithImportedNamespace()
    {
        var source = """
                     using AutoScoper;
                     
                     namespace TestNamespace;

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
                     namespace TestNamespace;
                     
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
                     namespace TestNamespace;
                     
                     [AutoScope]
                     public partial class Test
                     {
                     }
                     """;

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task ShouldNotGenerate_WhenClassIsMarkedWithAttribute_InComplexNamespace()
    {
        var source = """
                     using AutoScoper;
                     
                     namespace TestNamespace.Complex.Nested;

                     [AutoScope]
                     public partial class Test
                     {
                     }
                     """;

        return TestHelper.Verify(source);
    }
}
