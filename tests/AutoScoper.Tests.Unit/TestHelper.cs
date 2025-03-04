﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AutoScoper.Tests.Unit;

public static class TestHelper
{
    public static Task Verify(string source)
    {
        // Parse the provided string into a C# syntax tree
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic)
            .Select(assembly => MetadataReference
                .CreateFromFile(assembly.Location))
            .Cast<MetadataReference>();

        // Create a Roslyn compilation for the syntax tree.
        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree },
            references: references);


        // Create an instance of our EnumGenerator incremental source generator
        var generator = new AutoScopeGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out _);

        // Use verify to snapshot test the source generator output!
        return Verifier
            .Verify(driver)
            .UseDirectory("Snapshots");;
    }
}
