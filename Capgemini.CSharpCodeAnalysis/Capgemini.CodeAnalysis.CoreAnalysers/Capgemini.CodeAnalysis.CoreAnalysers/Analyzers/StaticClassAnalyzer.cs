using System;
using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// This analyzer implements the following code review rule: Static classes must be avoided if they perform calculations that can vary as this can affect testing.
    /// <para>If the class it truly suitable for a static class, this rule can safely be suppressed.</para>
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticClassAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
                                                                                    AnalyzerType.StaticClassAnalyzerId.ToDiagnosticId(),
                                                                                    nameof(StaticClassAnalyzer),
                                                                                    $"{nameof(StaticClassAnalyzer)}: {{0}}",
                                                                                    AnalyserCategoryConstants.StaticAnalyzer,
                                                                                    DiagnosticSeverity.Warning,
                                                                                    true);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context), $"An instance of {nameof(context)} was not supplied.");
            }

            context.RegisterSyntaxNodeAction(AnalysisDeclaration, SyntaxKind.ClassDeclaration);
        }

        private static bool IsAnExtensionClass(ClassDeclarationSyntax declaration)
        {
            var isExtensionClass = false;
            var methods = declaration.Members.OfType<MethodDeclarationSyntax>().ToList();
            foreach (var method in methods)
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    if (parameter.Modifiers.Any(SyntaxKind.ThisKeyword))
                    {
                        isExtensionClass = true;
                        break;
                    }
                }

                if (isExtensionClass)
                {
                    break;
                }
            }

            return isExtensionClass;
        }

        private void AnalysisDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            if (declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                var isExtensionClass = IsAnExtensionClass(declaration);

                if (!isExtensionClass)
                {
                    DiagnosticsManager.CreateStaticClassDiagnostic(context, declaration.Identifier.GetLocation(), Rule);
                }
            }
        }
    }
}