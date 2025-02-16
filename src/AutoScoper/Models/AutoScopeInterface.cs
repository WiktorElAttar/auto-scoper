using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace AutoScoper.Models;

internal class AutoScopeInterface
{
    internal INamedTypeSymbol InterfaceSymbol { get; set; }

    internal List<AutoScopeMethod> Methods { get; set; } = [];
}
