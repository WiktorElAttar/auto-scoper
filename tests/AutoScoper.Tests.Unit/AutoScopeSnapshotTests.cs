namespace AutoScoper.Tests.Unit;

public class AutoScopeSnapshotTests
{
    [Fact]
    public Task ShouldGenerate_WhenClassMarkedWithAttribute_WithImportedNamespace()
    {
        var source = """
                     using AutoScoper;
                     
                     namespace TestNamespace;

                     [AutoScope(typeof(ITestInterface))]
                     public partial class Test
                     {
                     }
                     
                     [AutoScope(typeof(ITestInterface))]
                     public partial class Test2
                     {
                     }
                     
                     public interface ITestInterface
                     {
                         int GetInt();
                     }
                     """;

        return TestHelper.Verify(source);
    }

//     [Fact]
//     public Task ShouldGenerate_WhenClassIsMarkedWithAttribute_UsingFullName()
//     {
//         var source = """
//                      namespace TestNamespace;
//
//                      [AutoScoper.AutoScope(typeof(ITestInterface))]
//                      public partial class Test
//                      {
//                      }
//
//                      public interface ITestInterface
//                      {
//                          int GetInt(int a, string asd);
//                      }
//                      """;
//
//         return TestHelper.Verify(source);
//     }
//
//     [Fact]
//     public Task ShouldNotGenerate_WhenClassIsMarkedWithAttribute_WithoutNamespaceImport()
//     {
//         var source = """
//                      namespace TestNamespace;
//
//                      [AutoScope(typeof(ITestInterface))]
//                      public partial class Test
//                      {
//                      }
//
//                      public interface ITestInterface
//                      {
//                          int GetInt();
//                      }
//                      """;
//
//         return TestHelper.Verify(source);
//     }
//
//     [Fact]
//     public Task ShouldNotGenerate_WhenClassIsMarkedWithAttribute_InComplexNamespace()
//     {
//         var source = """
//                      using AutoScoper;
//
//                      namespace TestNamespace.Complex.Nested;
//
//                      [AutoScope(typeof(ITestInterface))]
//                      public partial class Test
//                      {
//                      }
//
//                      public interface ITestInterface
//                      {
//                          int GetInt();
//                      }
//                      """;
//
//         return TestHelper.Verify(source);
//     }
}
