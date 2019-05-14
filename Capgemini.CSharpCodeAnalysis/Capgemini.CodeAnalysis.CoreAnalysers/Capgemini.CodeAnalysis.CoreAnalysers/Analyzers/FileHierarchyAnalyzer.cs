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
    ///Files must be organised neatly in a structured, hierarchical manner within the Visual Studio solution using folders named following the namespace hierarchy.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FileHierarchyAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.FileHierarchyAnalyzerId.ToDiagnosticId(), nameof(FileHierarchyAnalyzer),
            $"{nameof(FileHierarchyAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        /// <summary>
        /// Overrides the Supported Diagnostics property
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
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