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
    /// Enforces that excessive amounts of comments are not added to any part of the code base.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LargeCommentedCodeAnalyzer : AnalyzerBase
    {
        private const int MaxNoOfLinesForComments = 20;
        private const string Message = "These lines of code are redundant. Please delete them.";

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                                    AnalyzerType.LargeCommentedCodeAnalyzerId.ToDiagnosticId(),
                                    nameof(LargeCommentedCodeAnalyzer),
                                    $"{nameof(LargeCommentedCodeAnalyzer)}: {{0}}",
                                    AnalyserCategoryConstants.Comments,
                                    DiagnosticSeverity.Error,
                                    true);

        /// <summary>
        /// Gets the Supported Diagnostics property (which is overridden).
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context), $"An instance of {nameof(context)} was not supplied.");
            }

            context.RegisterCompilationStartAction(CompilationStartAction);
        }

        private static void ProcessSingleLineComments(SyntaxNodeAnalysisContext context, List<SyntaxTrivia> singleLineComments, string declarationBodyText = null)
        {
            if (singleLineComments?.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(declarationBodyText) && RegexManager.MatchFound($@"((\s)*\/\/(.)*(\n)*){{{MaxNoOfLinesForComments},}}", declarationBodyText))
                {
                    foreach (var singleLineComment in singleLineComments)
                    {
                        DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, singleLineComment.GetLocation(), Rule, Message);
                    }
                }
                else if (singleLineComments.Count > MaxNoOfLinesForComments)
                {
                    foreach (var singleLineComment in singleLineComments)
                    {
                        DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, singleLineComment.GetLocation(), Rule, Message);
                    }
                }
            }
        }

        private static bool ProcessMultiLineComments(SyntaxNodeAnalysisContext context, List<SyntaxTrivia> multiLineComments)
        {
            var multiLineCommentViolationFound = false;
            if (multiLineComments?.Count > 0)
            {
                foreach (var multiLineComment in multiLineComments)
                {
                    var content = multiLineComment.ToFullString();
                    if (RegexManager.MatchFound($@"\/[*]((.)*\n){{{MaxNoOfLinesForComments},}}", content))
                    {
                        DiagnosticsManager.CreateCommentsTooLongDiagnostic(context, multiLineComment.GetLocation(), Rule, Message);
                        multiLineCommentViolationFound = true;
                        break;
                    }
                }
            }

            return multiLineCommentViolationFound;
        }

        private static void ProcessComments(SyntaxNodeAnalysisContext context, List<SyntaxTrivia> comments, string declarationBodyText = null)
        {
            if (comments?.Count > 0)
            {
                if (!ProcessMultiLineComments(context, comments.Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia)).ToList()))
                {
                    ProcessSingleLineComments(context, comments.Where(a => a.IsKind(SyntaxKind.SingleLineCommentTrivia)).ToList(), declarationBodyText);
                }
            }
        }

        private void CompilationStartAction(CompilationStartAnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeFieldDeclaration, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzePropertyDeclaration, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<FieldDeclarationSyntax>(context.Node);

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                           .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                           .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }

        private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<PropertyDeclarationSyntax>(context.Node);

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                           .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                           .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            var comments = declaration.CloseBraceToken.GetAllTrivia()?
                                                      .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                      .ToList();

            ProcessComments(context, comments, declaration.ToFullString());

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                                                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                       .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }

        private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<InterfaceDeclarationSyntax>(context.Node);
            var comments = declaration.CloseBraceToken.GetAllTrivia()?
                                                      .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                      .ToList();

            ProcessComments(context, comments, declaration.ToFullString());

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                                                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                       .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }

        private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);
            var comments = declaration.Body.CloseBraceToken.GetAllTrivia()?
                           .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                           .ToList();

            ProcessComments(context, comments, declaration.Body?.ToFullString());

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                                                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                       .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            var comments = declaration.Body?.CloseBraceToken.GetAllTrivia()?
                             .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                             .ToList();

            ProcessComments(context, comments, declaration.Body?.ToFullString());

            var commentsBeforeDeclaration = declaration.GetLeadingTrivia()
                                                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                                                       .ToList();
            ProcessComments(context, commentsBeforeDeclaration);
        }
    }
}
