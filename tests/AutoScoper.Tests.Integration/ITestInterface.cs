namespace AutoScoper.Tests.Integration;

public interface ITestInterface
{
    int GetInt(int a, string b);
}

public interface ITestInterface2
{
    Task<int> GetIntAsync(int a, string b);
}
public interface ITestInterface3
{
    Task GetTAsync<T>(int a, string b);
}
