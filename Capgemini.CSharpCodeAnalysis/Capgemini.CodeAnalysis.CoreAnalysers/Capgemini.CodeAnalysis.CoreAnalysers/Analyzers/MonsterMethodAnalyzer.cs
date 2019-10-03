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
    /// This analyzer implements the following code review rule: No big/monster methods - break down big methods into a few smaller methods with meaningful names, so the code is self-descriptive and does not require excessive comments.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MonsterMethodAnalyzer : AnalyzerBase
    {
        private const int MethodMaxLine = 80;

        private static readonly DiagnosticDescriptor Rule =
                                                            new DiagnosticDescriptor(
                                                                    AnalyzerType.MonsterMethodAnalyzerId.ToDiagnosticId(),
                                                                    nameof(MonsterMethodAnalyzer),
                                                                    $"{nameof(MonsterMethodAnalyzer)}: {{0}}",
                                                                    AnalyserCategoryConstants.CodeStructure,
                                                                    DiagnosticSeverity.Error,
                                                                    true);

        /// <summary>
        /// Overrides the Supported Diagnostics property.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            var numberOfStatements = declaration.Body?.Statements.Count;

            if (numberOfStatements != null && numberOfStatements.Value > MethodMaxLine)
            {
                var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"This method is longer than {MethodMaxLine} lines of executable code. Please consider splitting this method into smaller methods.");
                context.ReportDiagnostic(diagnostics);
            }
        }
    }
}