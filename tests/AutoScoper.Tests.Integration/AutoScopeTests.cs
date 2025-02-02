namespace AutoScoper.Tests.Integration;

public class AutoScopeTests
{
    [Fact]
    public void ShouldGenerate_WhenClassIsMarkedWithAttribute()
    {
        // Arrange
        var sut = new TestClass();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => sut.GetInt(1, ""));
    }
}
