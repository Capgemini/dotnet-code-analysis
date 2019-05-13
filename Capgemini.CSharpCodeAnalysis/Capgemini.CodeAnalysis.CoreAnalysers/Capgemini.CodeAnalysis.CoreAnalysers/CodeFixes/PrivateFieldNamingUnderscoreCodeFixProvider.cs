using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Rename;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
        /// <summary>
    /// Implements the Private Field Naming Underscore CodeFixProvider
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PrivateFieldNamingUnderscoreCodeFixProvider)), Shared]
    public class PrivateFieldNamingUnderscoreCodeFixProvider : CodeFixProviderBase
    {
        /// <summary>
        /// Ovverrides FixableDiagnosticIds
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId()); }
        }

        /// <summary>
        /// Ovverrides RegisterCodeFixesAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var contextRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var firstDiagnostic = context.Diagnostics.First();
            var privateField = contextRoot.FindToken(firstDiagnostic.Location.SourceSpan.Start);
            context.RegisterCodeFix(
                CodeAction.Create($"Prepend `_` to field '{privateField}'", 
                    cancellationToken => PrependUnderscore(context.Document, privateField, cancellationToken), 
                    AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId()), firstDiagnostic);
        }

        async Task<Solution> PrependUnderscore(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            var newName = $"_{declaration.ValueText}";
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            var solution = document.Project.Solution;

            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }
    }
}