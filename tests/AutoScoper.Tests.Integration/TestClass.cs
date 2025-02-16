namespace AutoScoper.Tests.Integration;

[AutoScope(typeof(ITestInterface), typeof(ITestInterface2), typeof(ITestInterface3))]
public partial class TestClass : ITestInterface
{
    public partial int GetInt(int a, string b);
}
