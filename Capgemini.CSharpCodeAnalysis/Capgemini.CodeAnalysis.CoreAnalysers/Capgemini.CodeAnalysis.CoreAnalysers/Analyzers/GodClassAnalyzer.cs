using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// "God" classes are not allowed - each class should be small, self-contained and do only one thing well (follow single responsibility pattern from SOLID principles). Anybody reading the code must be able to read a complete method from start to end without scrolling in the window
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GodClassAnalyzer : AnalyzerBase
    {
        private const int ClassMaxNumberOfPublicMethods = 20;

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyserConstants.GodClassAnalyzerId, nameof(GodClassAnalyzer),
            $"{nameof(GodClassAnalyzer)} \'{{0}}\'", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeDeclararion, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeDeclararion(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            var methodCount = declaration.Members.OfType<MethodDeclarationSyntax>().Count(a => !IsPrivate(a.Modifiers));

            if (methodCount > ClassMaxNumberOfPublicMethods)
            {
                var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"This class has {methodCount} methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.");
                context.ReportDiagnostic(diagnostics);
            }
        }
    }
}