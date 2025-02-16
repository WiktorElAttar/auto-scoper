using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoScoper.Extensions;

public static class RoslynExtensions
{
    public static INamedTypeSymbol? GetBestTypeByMetadataName(this Compilation compilation, string fullyQualifiedMetadataName)
    {
        // Try to get the unique type with this name, ignoring accessibility
        var type = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);

        // Otherwise, try to get the unique type with this name originally defined in 'compilation'
        type ??= compilation.Assembly.GetTypeByMetadataName(fullyQualifiedMetadataName);

        // Otherwise, try to get the unique accessible type with this name from a reference
        if (type is null)
        {
            foreach (var module in compilation.Assembly.Modules)
            {
                foreach (var referencedAssembly in module.ReferencedAssemblySymbols)
                {
                    var currentType = referencedAssembly.GetTypeByMetadataName(fullyQualifiedMetadataName);
                    if (currentType is null)
                        continue;

                    switch (currentType.GetResultantVisibility())
                    {
                        case SymbolVisibility.Public:
                        case SymbolVisibility.Internal when referencedAssembly.GivesAccessTo(compilation.Assembly):
                            break;

                        default:
                            continue;
                    }

                    if (type is object)
                    {
                        // Multiple visible types with the same metadata name are present
                        return null;
                    }

                    type = currentType;
                }
            }
        }

        return type;
    }

    // Taken from LoggerMessageGenerator.Parser
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/gen/LoggerMessageGenerator.Parser.cs
    public static string GetContainingNamespace(this ClassDeclarationSyntax classDeclaration)
    {
        var nspace = string.Empty;

        // determine the namespace the class is declared in, if any
        SyntaxNode? potentialNamespaceParent = classDeclaration.Parent;
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax &&
               potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
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
    public static string GenerateClassName(this TypeDeclarationSyntax typeDeclaration)
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



     // copied from https://github.com/dotnet/roslyn/blob/main/src/Workspaces/SharedUtilitiesAndExtensions/Compiler/Core/Extensions/ISymbolExtensions.cs
    private static SymbolVisibility GetResultantVisibility(this ISymbol symbol)
    {
        // Start by assuming it's visible.
        SymbolVisibility visibility = SymbolVisibility.Public;

        switch (symbol.Kind)
        {
            case SymbolKind.Alias:
                // Aliases are uber private.  They're only visible in the same file that they
                // were declared in.
                return SymbolVisibility.Private;

            case SymbolKind.Parameter:
                // Parameters are only as visible as their containing symbol
                return GetResultantVisibility(symbol.ContainingSymbol);

            case SymbolKind.TypeParameter:
                // Type Parameters are private.
                return SymbolVisibility.Private;
        }

        while (symbol != null && symbol.Kind != SymbolKind.Namespace)
        {
            switch (symbol.DeclaredAccessibility)
            {
                // If we see anything private, then the symbol is private.
                case Accessibility.NotApplicable:
                case Accessibility.Private:
                    return SymbolVisibility.Private;

                // If we see anything internal, then knock it down from public to
                // internal.
                case Accessibility.Internal:
                case Accessibility.ProtectedAndInternal:
                    visibility = SymbolVisibility.Internal;
                    break;

                    // For anything else (Public, Protected, ProtectedOrInternal), the
                    // symbol stays at the level we've gotten so far.
            }

            symbol = symbol.ContainingSymbol;
        }

        return visibility;
    }

    // Copied from: https://github.com/dotnet/roslyn/blob/main/src/Workspaces/SharedUtilitiesAndExtensions/Compiler/Core/Utilities/SymbolVisibility.cs
    private enum SymbolVisibility
    {
        Public = 0,
        Internal = 1,
        Private = 2,
        Friend = Internal,
    }
}
