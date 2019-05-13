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
    /// Cyclomatic complexity attempts to determine the number of different paths within a method. To achieve this, 
    /// this analyser first assign a method a score of 1 as the default cyclomatic complexity. Subsequently, the analyser 
    /// identifiers the following contructs within a method: if ; while ; for ; foreach ; case ; default ; continue ; &amp;&amp; ; || ; catch ; ?: ; ??
    /// For each of these contruct found, a score of 1 is added to the method cyclomatic complexity.The total score yields 
    /// the method's cyclomatic complexity. Cyclomatic complexity score greater than 15 results to an error.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodComplexityAnalyzer: AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.MethodComplexityAnalyzeId.ToDiagnosticId(), nameof(MethodComplexityAnalyzer),
            $"{nameof(MethodComplexityAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);
        
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
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }
        
        private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            var cyclometricComplexity = 1;

            cyclometricComplexity += declaration.DescendantNodes().OfType<IfStatementSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<WhileStatementSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<ForStatementSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<ForEachStatementSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<CaseSwitchLabelSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<DefaultSwitchLabelSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<ContinueStatementSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<CatchClauseSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<ConditionalExpressionSyntax>().ToList().Count;
            cyclometricComplexity += declaration.DescendantNodes().OfType<BinaryExpressionSyntax>().Where(a=>a.IsKind(SyntaxKind.CoalesceExpression)).ToList().Count;
            cyclometricComplexity += declaration.DescendantTokens().Where(a => a.IsKind(SyntaxKind.AmpersandAmpersandToken) || a.IsKind(SyntaxKind.BarBarToken)).ToList().Count;

            if (cyclometricComplexity > 15)
            {
                var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"The cyclometric complexity of this method method is {cyclometricComplexity} which is greater than the maximum value of 15. Please consider splitting this method into smaller methods.");
                context.ReportDiagnostic(diagnostics);
            }
        }
    }
}
