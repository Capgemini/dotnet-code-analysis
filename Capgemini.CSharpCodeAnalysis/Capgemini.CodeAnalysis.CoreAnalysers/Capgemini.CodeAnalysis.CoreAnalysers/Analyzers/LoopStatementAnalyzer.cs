using System.Collections.Immutable;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// Enforces that all loop statements are guarded by curly braces
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LoopStatementAnalyzer : AnalyzerBase
    {
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyserConstants.LoopStatementAnalyzerId, nameof(LoopStatementAnalyzer),
            $"{nameof(LoopStatementAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        /// <summary>
        /// Returns a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeForEachStatement, SyntaxKind.ForEachStatement);
            context.RegisterSyntaxNodeAction(AnalyzeForStatement, SyntaxKind.ForStatement);
            context.RegisterSyntaxNodeAction(AnalyzeWhileStatement, SyntaxKind.WhileStatement);
        }

        private void AnalyzeForEachStatement(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax>(context.Node);
            
            if (!declaration.Statement.IsKind(SyntaxKind.Block))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.ForEachKeyword.GetLocation(), "Please ensure that foreach statements have corresponding curly braces."));
            }
        }

        private void AnalyzeForStatement(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax>(context.Node);
            
            if (!declaration.Statement.IsKind(SyntaxKind.Block))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.ForKeyword.GetLocation(), "Please ensure that for statements have corresponding curly braces."));
            }
        }
        private void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<Microsoft.CodeAnalysis.CSharp.Syntax.WhileStatementSyntax>(context.Node);
            
            if (!declaration.Statement.IsKind(SyntaxKind.Block))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.WhileKeyword.GetLocation(), "Please ensure that while statements have corresponding curly braces."));
            }
        }
    }
}