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
    /// Enforces that only one type is defined within a C# file.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class OneTypePerFileAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule =
                                                            new DiagnosticDescriptor(
                                                                    AnalyzerType.OneTypePerFileAnalyzerId.ToDiagnosticId(),
                                                                    nameof(OneTypePerFileAnalyzer),
                                                                    $"{nameof(OneTypePerFileAnalyzer)}: {{0}}",
                                                                    AnalyserCategoryConstants.CodeStructure,
                                                                    DiagnosticSeverity.Error,
                                                                    true);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

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