using System;
using System.Collections.Immutable;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// Files must be organised neatly in a structured, hierarchical manner within the Visual Studio solution using folders named following the namespace hierarchy.
    /// All tests have been removed as, now this is deprecated, they fail and changes are not supported - deprecated ;-).
    /// </summary>
    [Obsolete("Please use StyleCop.Analyzers instead. This analyser will be removed in future versions. This analyser is now disabled by default.")]
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FileHierarchyAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule =
                                                            new DiagnosticDescriptor(
                                                                                    AnalyzerType.FileHierarchyAnalyzerId.ToDiagnosticId(),
                                                                                    nameof(FileHierarchyAnalyzer),
                                                                                    $"{nameof(FileHierarchyAnalyzer)}: {{0}}",
                                                                                    AnalyserCategoryConstants.CodeStructure,
                                                                                    DiagnosticSeverity.Error,
                                                                                    false);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context), $"An instance of {nameof(context)} was not supplied.");
            }

            context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<NamespaceDeclarationSyntax>(context.Node);
            var filePath = declaration.SyntaxTree.FilePath.Replace("\\", ".");

            if (!filePath.Contains($"{declaration.Name}."))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.Name.GetLocation(), "Namespace should match against file structure."));
            }
        }
    }
}