using System;
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
    /// This analyzer implements the following code review rule:
    ///     All members (constructors, methods, properties, fields, etc) that are marked with access modifier which makes them accessible from outside the file itself must have XML comments,
    ///     including meaningful, readable and plain-English descriptions and details of parameters and return values.
    ///     This includes Public, Protected and Internal members and applies to both Interfaces and their corresponding class implementations.
    /// All tests have been removed as, now this is deprecated, they fail and changes are not supported - deprecated ;-).
    /// </summary>
    [Obsolete("Please use StyleCop.Analyzers instead. This analyser will be removed in future versions. This analyser is now disabled by default.")]
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class XmlCommentsAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.XmlCommentsAnalyzerId.ToDiagnosticId(), nameof(XmlCommentsAnalyzer),
            $"{nameof(XmlCommentsAnalyzer)} \'{{0}}\'", AnalyserCategoryConstants.Comments, DiagnosticSeverity.Warning, false);

        /// <summary>
        /// Returns a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzedMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
        }

        private void AnalyzedInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<InterfaceDeclarationSyntax>(context.Node);

            if (!CommentsManager.HasValidSummaryComments(context.Node))
            {
                DiagnosticsManager.CreateCommentsDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzedClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);

            if (IsExternallyVisibleComments(declaration.Modifiers) && !CommentsManager.HasValidSummaryComments(context.Node))
            {
                DiagnosticsManager.CreateCommentsDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzedMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);

            // if this method is within an interface then we do not need to process with access qualifier check
            var interfaceDeclaration = Cast<InterfaceDeclarationSyntax>(declaration.Parent);
            if (((interfaceDeclaration == null && IsExternallyVisibleComments(declaration.Modifiers)) || interfaceDeclaration != null) &&
                 !CommentsManager.HasValidSummaryComments(context.Node))
            {
                DiagnosticsManager.CreateCommentsDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }
    }
}