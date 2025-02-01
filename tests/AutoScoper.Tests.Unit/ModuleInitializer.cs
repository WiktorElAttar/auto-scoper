using System.Runtime.CompilerServices;

namespace AutoScoper.Tests.Unit;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}
