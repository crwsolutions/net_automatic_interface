﻿using System;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AutomaticInterface;

[Generator]
public class AutomaticInterfaceGenerator : IIncrementalGenerator
{
    public const string DefaultAttributeName = "GenerateAutomaticInterface";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static postInitializationContext =>
        {
            postInitializationContext.AddSource(
                $"{DefaultAttributeName}.Attribute.g.cs",
                SourceText.From(
                    $$$"""
                    // <auto-generated />
                    using System;

                    namespace AutomaticInterface
                    {
                        /// <summary>
                        /// Use source generator to automatically create a Interface from this class
                        /// </summary>
                        [AttributeUsage(AttributeTargets.Class)]
                        internal sealed class {{{DefaultAttributeName}}}Attribute : Attribute
                        {
                            internal {{{DefaultAttributeName}}}Attribute(string namespaceName = "") { }
                        }
                    }
                    """,
                    Encoding.UTF8
                )
            );
        });

        var classes = context
            .SyntaxProvider.CreateSyntaxProvider(CouldBeClassWithInterfaceAttribute, Transform)
            .Where(type => type is not null)
            .Collect();

        context.RegisterSourceOutput(classes, GenerateCode);
    }

    private static bool CouldBeClassWithInterfaceAttribute(
        SyntaxNode syntaxNode,
        CancellationToken _
    )
    {
        if (syntaxNode is not AttributeSyntax attribute)
        {
            return false;
        }

        var name = ExtractName(attribute.Name);

        return name is DefaultAttributeName;
    }

    private static string? ExtractName(NameSyntax? name)
    {
        return name switch
        {
            SimpleNameSyntax ins => ins.Identifier.Text,
            QualifiedNameSyntax qns => qns.Right.Identifier.Text,
            _ => null
        };
    }

    private static void GenerateCode(
        SourceProductionContext context,
        ImmutableArray<ITypeSymbol?> enumerations
    )
    {
        if (enumerations.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var type in enumerations)
        {
            if (type is null)
            {
                continue;
            }

            var typeNamespace = type.ContainingNamespace.IsGlobalNamespace
                ? $"${Guid.NewGuid().ToString()}"
                : $"{type.ContainingNamespace}";

            var code = Builder.BuildInterfaceFor(type);

            var hintName = $"{typeNamespace}.I{type.Name}";
            context.AddSource(hintName, code);
        }
    }

    private static ITypeSymbol? Transform(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken
    )
    {
        var attributeSyntax = (AttributeSyntax)context.Node;
        if (attributeSyntax.Parent?.Parent is not ClassDeclarationSyntax classDeclaration)
        {
            return null;
        }

        var type =
            context.SemanticModel.GetDeclaredSymbol(
                classDeclaration,
                cancellationToken: cancellationToken
            ) as ITypeSymbol;
        return type;
    }
}
