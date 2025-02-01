using System.Runtime.CompilerServices;

namespace AutoScopper.Tests.Unit;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}
