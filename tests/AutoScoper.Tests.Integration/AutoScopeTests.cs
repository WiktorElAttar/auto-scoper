namespace AutoScoper.Tests.Integration;

public class AutoScopeTests
{
    [Fact]
    public void ShouldGenerate_WhenClassIsMarkedWithAttribute()
    {
        // Arrange
        var sut = new TestClass();

        // Act
        var result = sut.GetInt();

        // Assert
        Assert.Equal(1, result);
    }
}
