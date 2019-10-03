using System;
using System.Collections.Generic;
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
    ///  This analyzer implements the following code review rule: Code must not need lots of extra comments.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CommentsAnalyzer : AnalyzerBase
    {
        private const int MaxNumberOfLinesForDocumentation = 30;

        private static readonly DiagnosticDescriptor Rule =
                                                            new DiagnosticDescriptor(
                                                                AnalyzerType.CommentsAnalyzerId.ToDiagnosticId(),
                                                                nameof(CommentsAnalyzer),
                                                                $"{nameof(CommentsAnalyzer)}: {{0}}",
                                                                AnalyserCategoryConstants.Comments,
                                                                DiagnosticSeverity.Error,
                                                                true);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzePropertyDeclaration, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeLocalVariableDeclaration, SyntaxKind.LocalDeclarationStatement);
        }

        private static void ProcessDocumentationComments(SyntaxNodeAnalysisContext context, Location declarationLocation, string declarationName, int threshold)
        {
            var docummentation = CommentsManager.ExtractDocumentationComment(context.Node);
            if (docummentation != null)
            {
                var numberOfLines = RegexManager.NumberOfLines(docummentation.Content.ToFullString());
                if (numberOfLines > threshold)
                {
                    DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, declarationLocation, Rule, declarationName, threshold);
                }
            }
        }

        private static void ProcessComments(SyntaxNodeAnalysisContext context, List<SyntaxTrivia> comments, Location objectLocation, string objectName)
        {
            const int maxNumberOfLinesForComments = 5;
            var singleLineComments = comments.Where(a => a.IsKind(SyntaxKind.SingleLineCommentTrivia)).ToList();
            var multiLineComments = comments.Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia)).ToList();

            if (singleLineComments.Count > 0 && multiLineComments.Count > 0)
            {
                DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, objectLocation, Rule, $"{objectName} has both multiline and single line comments. Please use only one type of comments.");
            }
            else if (singleLineComments.Count > maxNumberOfLinesForComments)
            {
                DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, objectLocation, Rule, $"{objectName} has more than {maxNumberOfLinesForComments} lines comments. Please use no more than {maxNumberOfLinesForComments} lines of comments.");
            }
            else if (multiLineComments.Count > 1)
            {
                DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, objectLocation, Rule, $"{objectName} has multiple MultiLines comments. Please use no more than 1 MultiLines of comments.");
            }
            else if (multiLineComments.Count == 1)
            {
                var numberOfLines = RegexManager.NumberOfLines(multiLineComments[0].ToFullString());
                if (numberOfLines > maxNumberOfLinesForComments)
                {
                    DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, objectLocation, Rule, $"{objectName} has more than {maxNumberOfLinesForComments} lines comments. Please use no more than {maxNumberOfLinesForComments} lines of comments.");
                }
            }
        }

        private static void AnalyzeComments(SyntaxNodeAnalysisContext context, SyntaxToken syntaxToken, string message)
        {
            var leadingComments = context.Node.GetLeadingTrivia()
                .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                .ToList();
            var trailingComments = context.Node.GetTrailingTrivia()
                .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                .ToList();

            if (leadingComments.Count > 0 && trailingComments.Count > 0)
            {
                // we don't want a variable to have both leading and trailing comments
                DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, syntaxToken.GetLocation(), Rule, message);
            }
            else if (leadingComments.Count > 0)
            {
                ProcessComments(context, leadingComments, syntaxToken.GetLocation(), syntaxToken.Text);
            }
            else if (trailingComments.Count > 0)
            {
                ProcessComments(context, trailingComments, syntaxToken.GetLocation(), syntaxToken.Text);
            }
        }

        private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<PropertyDeclarationSyntax>(context.Node);
            ProcessDocumentationComments(context, declaration.Identifier.GetLocation(), declaration.Identifier.Text, MaxNumberOfLinesForDocumentation);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            ProcessDocumentationComments(context, declaration.Identifier.GetLocation(), declaration.Identifier.Text, MaxNumberOfLinesForDocumentation);
        }

        private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);
            ProcessDocumentationComments(context, declaration.Identifier.GetLocation(), declaration.Identifier.Text, MaxNumberOfLinesForDocumentation);
        }

        private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<InterfaceDeclarationSyntax>(context.Node);
            ProcessDocumentationComments(context, declaration.Identifier.GetLocation(), declaration.Identifier.Text, MaxNumberOfLinesForDocumentation);
        }

        private void AnalyzeLocalVariableDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<LocalDeclarationStatementSyntax>(context.Node);
            var identifier = declaration.Declaration.Variables.First().Identifier;
            AnalyzeComments(context, identifier, $"{identifier.Text} has both leading and trailing comments. Only one type is allowed.");
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            var identifier = declaration.Identifier;
            ProcessDocumentationComments(context, identifier.GetLocation(), identifier.Text, MaxNumberOfLinesForDocumentation);
        }
    }
}