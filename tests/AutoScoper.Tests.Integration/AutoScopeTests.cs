using Microsoft.Extensions.DependencyInjection;

namespace AutoScoper.Tests.Integration;

public class AutoScopeTests
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddScoped<ITestInterface, TestClassImpl>()
        .AddScoped<ITestInterface2, TestClassImpl>()
        .BuildServiceProvider();

    [Fact]
    public void ShouldGenerate_WhenClassIsMarkedWithAttribute()
    {
        // Arrange
        AutoScopeProvider.ServiceProvider = _serviceProvider;
        var sut = new TestClass();

        // Act
        var result = sut.GetInt(1, "1");

        // Assert
        Assert.Equal(2, result);
    }
}
