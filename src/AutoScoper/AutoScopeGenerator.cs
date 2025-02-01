using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AutoScoper;

[Generator]
public class AutoScopeGenerator: IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            AutoScopeAttribute.FileName,
            SourceText.From(AutoScopeAttribute.Attribute, Encoding.UTF8)));

        var scopesToGenerate = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AutoScopeAttribute.FullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => CreateAutoScopeDetails(ctx))
            .Where(m => m != null);

        context.RegisterSourceOutput(
            scopesToGenerate,
            static (ctx, scopeDetails) => Execute(ctx, scopeDetails));
    }

    private static ScopeDetails CreateAutoScopeDetails(GeneratorAttributeSyntaxContext ctx)
        => new (ctx.TargetSymbol.Name, ctx.TargetSymbol.ContainingNamespace.ToString());

    private static void Execute(SourceProductionContext ctx, ScopeDetails scopeDetails)
    {
        var result = GenerateAutoScopedPartialClass(scopeDetails);

        ctx.AddSource(
            $"AutoScoper.{scopeDetails.ClassName}.g.cs",
            SourceText.From(result, Encoding.UTF8));
    }

    private static string GenerateAutoScopedPartialClass(ScopeDetails scopeDetails)
    {
        var sb = new StringBuilder();

        sb.Append($$"""
namespace {{scopeDetails.NamespaceName}};

public partial class {{scopeDetails.ClassName}}
{
    public int GetInt() => 1;
}
""");

        return sb.ToString();
    }
}

public class ScopeDetails(
    string className,
    string namespaceName)
{
    public string ClassName { get; } = className;
    public string NamespaceName { get; } = namespaceName;
};

