namespace AutoScopper.Tests.Unit;

public class AutoScopeSnapshotTests
{
    [Fact]
    public Task Test1()
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
}
