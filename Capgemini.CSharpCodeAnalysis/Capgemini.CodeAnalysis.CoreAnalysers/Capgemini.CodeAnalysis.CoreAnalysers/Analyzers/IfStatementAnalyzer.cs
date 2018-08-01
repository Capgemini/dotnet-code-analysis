using System.Collections.Immutable;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using IfStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.IfStatementSyntax;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// Enforces that all If statements are guarded by curly braces
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfStatementAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.IfStatementAnalyzerId.ToDiagnosticId(), nameof(IfStatementAnalyzer),
            $"{nameof(IfStatementAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

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
            context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement); 
        }

        private void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<IfStatementSyntax>(context.Node);

            if (!declaration.Statement.IsKind(SyntaxKind.Block))
            {
                if (!declaration.Statement.GetText().ToString().Trim().StartsWith("return") &&
                    !declaration.Statement.GetText().ToString().Trim().StartsWith("continue;") &&
                    !declaration.Statement.GetText().ToString().Trim().StartsWith("break;")
                    
                    )
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.IfKeyword.GetLocation(), "Please ensure that If statements have corresponding curly braces."));
                }
            }
            
            if (declaration.Else != null )
            {
                if (!declaration.Else.Statement.IsKind(SyntaxKind.IfStatement) && !declaration.Else.Statement.IsKind(SyntaxKind.Block))
                {
                   context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.Else.ElseKeyword.GetLocation(), "Please ensure that Else statements have corresponding curly braces."));
               }
            }
        }
    }
}