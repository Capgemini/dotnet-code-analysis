using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.XrmAnalysers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SingleConstructornAnalyzer : AnalyzerBase
    {
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyserConstants.SingleConstructornAnalyzerId, nameof(SingleConstructornAnalyzer),
             $"{nameof(SingleConstructornAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<ClassDeclarationSyntax>(context.Node);

            var isInjectable = declaration.DescendantNodes().OfType<AttributeSyntax>()
                  .Any(x => x.Name.ToString().Replace("Attribute", "").Trim() == "Injectable");

            if (isInjectable)
            {
                var constructors = declaration.Members.OfType<ConstructorDeclarationSyntax>().ToList();
                if (constructors.Count > 1)
                {
                    var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"{declaration.Identifier.Text} already has a constructor. Only one constructor is allowed for this class.");
                    context.ReportDiagnostic(diagnostics);
                }
            }
        }
    }
}