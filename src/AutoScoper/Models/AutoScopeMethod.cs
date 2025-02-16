using Microsoft.CodeAnalysis;

namespace AutoScoper.Models;

internal class AutoScopeMethod(IMethodSymbol methodSymbol, bool isPartial)
{
    public IMethodSymbol MethodSymbol => methodSymbol;

    public bool IsPartial => isPartial;
}
