using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoScoper;

internal sealed class Parser(
    Compilation compilation,
    Action<Diagnostic> reportDiagnostic,
    CancellationToken cancellationToken)
{
    public IReadOnlyList<AutoScopeClass> GetAutoScopeClasses(ImmutableArray<GeneratorAttributeSyntaxContext> attributedClasses)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = new List<AutoScopeClass>();

        foreach (var classDeclaration in attributedClasses)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var autoScopeClass = GetAutoScopeClass(classDeclaration);
            result.Add(autoScopeClass);
        }

        return result;
    }

    private AutoScopeClass GetAutoScopeClass(GeneratorAttributeSyntaxContext attributedClass)
    {
        var classDeclaration = (ClassDeclarationSyntax)attributedClass.TargetNode;

        var namespaceName = GetContainingNamespace(classDeclaration);
        var className = GenerateClassName(classDeclaration);
        var interfaces = GetInterfaces(attributedClass.Attributes);
        var methods = GetMethods(interfaces, classDeclaration);

        AutoScopeClass result = new();

        return result;
    }

    // Taken from LoggerMessageGenerator.Parser
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/gen/LoggerMessageGenerator.Parser.cs
    private string GetContainingNamespace(ClassDeclarationSyntax classDeclaration)
    {
        var nspace = string.Empty;

        // determine the namespace the class is declared in, if any
        SyntaxNode? potentialNamespaceParent = classDeclaration.Parent;
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax
               && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax
              )
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            nspace = namespaceParent.Name.ToString();
            while (true)
            {
                namespaceParent = namespaceParent.Parent as NamespaceDeclarationSyntax;
                if (namespaceParent == null)
                {
                    break;
                }

                nspace = $"{namespaceParent.Name}.{nspace}";
            }
        }

        return nspace;
    }

    // Taken from LoggerMessageGenerator.Parser
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/gen/LoggerMessageGenerator.Parser.cs
    private static string GenerateClassName(TypeDeclarationSyntax typeDeclaration)
    {
        if (typeDeclaration.TypeParameterList != null &&
            typeDeclaration.TypeParameterList.Parameters.Count != 0)
        {
            // The source generator produces a partial class that the compiler merges with the original
            // class definition in the user code. If the user applies attributes to the generic types
            // of the class, it is necessary to remove these attribute annotations from the generated
            // code. Failure to do so may result in a compilation error (CS0579: Duplicate attribute).
            for (int i = 0; i < typeDeclaration.TypeParameterList?.Parameters.Count; i++)
            {
                TypeParameterSyntax parameter = typeDeclaration.TypeParameterList.Parameters[i];

                if (parameter.AttributeLists.Count > 0)
                {
                    typeDeclaration = typeDeclaration.ReplaceNode(parameter, parameter.WithAttributeLists([]));
                }
            }
        }

        return typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList;
    }

    private IReadOnlyList<string> GetInterfaces(ImmutableArray<AttributeData> attributes)
    {
        List<String> interfaces = [];

        foreach (var attribute in attributes)
        {
            foreach (var constructorArgument in attribute.ConstructorArguments)
            {
                foreach (var constructorArgumentValue in constructorArgument.Values)
                {
                    var typeSymbol = constructorArgumentValue.Value as INamedTypeSymbol;
                    if (typeSymbol is null)
                    {
                        continue;
                    }
                    interfaces.Add(typeSymbol.ToDisplayString());
                }
            }
        }

        return interfaces;
    }

    private IReadOnlyList<AutoScopeMethod> GetMethods(
        IReadOnlyList<string> interfaces,
        ClassDeclarationSyntax classDeclaration)
    {
        foreach (var interfaceName in interfaces)
        {
            var interfaceSymbol = compilation.GetBestTypeByMetadataName(interfaceName);

            if (interfaceSymbol is null)
            {
                continue;
            }

            foreach (var methodSymbol in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
            {

            }
        }

        return [];
    }
}

internal sealed class AutoScopeClass()
{
    public string Namespace { get; init; } = string.Empty;
    public string ClassName { get; init; } = string.Empty;
    public IReadOnlyList<string> Interfaces { get; init; } = [];
    public IReadOnlyList<AutoScopeMethod> Methods { get; init; } = [];
}

internal sealed class AutoScopeMethod()
{
}
