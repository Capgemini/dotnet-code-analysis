using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    /// <summary>
    /// Implements the Naming Convention CodeFixProvider.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamingConventionCodeFixProvider))]
    [Shared]
    public class NamingConventionCodeFixProvider : CodeFixProviderBase
    {
        /// <summary>
        /// Gets the overridden FixableDiagnosticIds.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AnalyzerType.NamingConventionAnalyzerId.ToDiagnosticId());

        /// <summary>
        /// Overrides RegisterCodeFixesAsync.
        /// </summary>
        /// <param name="context">An instance of <see cref="CodeFixContext"/> to support the analysis.</param>
        /// <returns>A task that, when completed, will contain the result of the Code Fix registration.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var x = diagnostic.Properties;
            var token = root.FindToken(diagnostic.Location.SourceSpan.Start);
            if (!root.ContainsDiagnostics)
            {
                // ToDo - add tests to see if this is valid and, more importantly, if the naming
                return;
            }

            context.RegisterCodeFix(
                CodeAction.Create("Pascal-case Field Name", c => PrependUnderscore(context.Document, token, c), AnalyzerType.NamingConventionAnalyzerId.ToDiagnosticId()),
                diagnostic);
        }

        private async Task<Solution> PrependUnderscore(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            var newName = $"_{declaration.ValueText}";
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            var solution = document.Project.Solution;

            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }
    }
}