using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class OneTypePerFileAnalyzer : AnalyzerBase
    {
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.OneTypePerFileAnalyzerId.ToDiagnosticId(), nameof(OneTypePerFileAnalyzer),
            $"{nameof(OneTypePerFileAnalyzer)} \'{{0}}\'", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

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
            context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<NamespaceDeclarationSyntax>(context.Node);

            var members = declaration.Members.Count(x => x.IsKind(SyntaxKind.ClassDeclaration) || 
                                                         x.IsKind(SyntaxKind.InterfaceDeclaration) || 
                                                         x.IsKind(SyntaxKind.EnumDeclaration));
            if (members > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.Name.GetLocation(), "Each file should contain only one type. Please split the types into multiple files."));
            }
        }
    }
}