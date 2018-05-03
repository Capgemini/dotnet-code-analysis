using System.Collections.Immutable;
using Capgemini.CodeAnalysis.Foundation;
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
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.FileHierarchyAnalyzerId.ToDiagnosticId(), nameof(FileHierarchyAnalyzer),
            $"{nameof(FileHierarchyAnalyzer)} \'{{0}}\'", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<NamespaceDeclarationSyntax>(context.Node);
            var filePath = declaration.SyntaxTree.FilePath.Replace("\\", ".");

            if (!filePath.Contains($"{declaration.Name}.")) 
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.Name.GetLocation(), "Namespace should match against file structure."));
            }
        }
    }
}