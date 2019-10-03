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
using Microsoft.CodeAnalysis.Rename;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    /// <summary>
    /// Implements the Private Field Naming Underscore CodeFixProvider.
    /// All tests have been removed as, now this is deprecated, they fail and changes are not supported - deprecated ;-).
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PrivateFieldNamingUnderscoreCodeFixProvider)), Shared]
    public class PrivateFieldNamingUnderscoreCodeFixProvider : CodeFixProviderBase
    {
        /// <summary>
        /// Overrides FixableDiagnosticIds.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId());

        /// <summary>
        /// Overrides RegisterCodeFixesAsync.
        /// </summary>
        /// <param name="context">An instance of <see cref="CodeFixContext"/> to support the analysis.</param>
        /// <returns></returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var contextRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var firstDiagnostic = context.Diagnostics.First();
            var privateField = contextRoot.FindToken(firstDiagnostic.Location.SourceSpan.Start);
            context.RegisterCodeFix(
                                    CodeAction.Create(
                                        $"Prepend `_` to field '{privateField}'",
                                        cancellationToken => PrependUnderscore(context.Document, privateField, cancellationToken),
                                        AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId()),
                                    firstDiagnostic);
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