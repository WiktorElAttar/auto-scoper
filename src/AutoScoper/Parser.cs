using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using AutoScoper.Extensions;
using AutoScoper.Models;
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

    private static AutoScopeClass GetAutoScopeClass(GeneratorAttributeSyntaxContext attributedClass)
    {
        var classDeclaration = (ClassDeclarationSyntax)attributedClass.TargetNode;
        var classSymbol = (INamedTypeSymbol)attributedClass.TargetSymbol;

        var namespaceName = classDeclaration.GetContainingNamespace();
        var className = classDeclaration.GenerateClassName();
        var interfaces = GetInterfaceSymbols(attributedClass.Attributes);
        GetMethods(interfaces, classSymbol);

        AutoScopeClass result = new()
        {
            Namespace = namespaceName,
            ClassName = className,
            ClassSymbol = classSymbol,
            Interfaces = interfaces,
        };

        return result;
    }

    private static List<AutoScopeInterface> GetInterfaceSymbols(ImmutableArray<AttributeData> attributes)
    {
        List<AutoScopeInterface> interfaces = [];

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

                    if (typeSymbol.TypeKind != TypeKind.Interface)
                    {
                        continue;
                    }

                    interfaces.Add(new(){ InterfaceSymbol = typeSymbol});
                }
            }
        }

        return interfaces;
    }

    private static void GetMethods(
        IReadOnlyList<AutoScopeInterface> interfaces,
        INamedTypeSymbol classSymbol)
    {
        List<IMethodSymbol> classMethods = [];
        foreach (var methodSymbol in classSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            classMethods.Add(methodSymbol);
        }

        foreach (var autoScopeInterface in interfaces)
        {
            foreach (var methodSymbol in autoScopeInterface.InterfaceSymbol.GetMembers().OfType<IMethodSymbol>())
            {
                var classMethod = classMethods.Find(x => x.Name == methodSymbol.Name);
                var isPartial = classMethod is not null;

                autoScopeInterface.Methods.Add(new AutoScopeMethod(methodSymbol, isPartial));
            }
        }
    }
}