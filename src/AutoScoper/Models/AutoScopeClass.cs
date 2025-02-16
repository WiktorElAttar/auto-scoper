using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace AutoScoper.Models;

internal sealed class AutoScopeClass()
{
    public string Namespace { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public INamedTypeSymbol ClassSymbol { get; set; }
    public List<AutoScopeInterface> Interfaces { get; set; } = [];
}
