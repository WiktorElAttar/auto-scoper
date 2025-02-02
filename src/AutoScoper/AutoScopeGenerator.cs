using System.Collections.Generic;
using System.Collections.Immutable;
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
             AutoScopeAttributeHelper.GeneratedFileName,
             SourceText.From(AutoScopeAttributeHelper.Attribute, Encoding.UTF8)));

        IncrementalValuesProvider<GeneratorAttributeSyntaxContext> classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AutoScopeAttributeHelper.FullName,
                (node, _) => node is ClassDeclarationSyntax,
                (ctx, _) => ctx);

        IncrementalValueProvider<(Compilation compilation, ImmutableArray<GeneratorAttributeSyntaxContext> classes)>
            compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(
            compilationAndClasses,
            static (ctx, source) => Execute(source.compilation, source.classes, ctx));
    }

    private static void Execute(
        Compilation compilation,
        ImmutableArray<GeneratorAttributeSyntaxContext> attributedClasses,
        SourceProductionContext context)
    {
        if (attributedClasses.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        var parser = new Parser(compilation, context.ReportDiagnostic, context.CancellationToken);

        IReadOnlyList<AutoScopeClass> autoScopeClasses = parser.GetAutoScopeClasses(attributedClasses);
    }
}
