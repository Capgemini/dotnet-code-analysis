using System.Composition;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamingConventionCodeFixProvider)), Shared]
    public class NamingConventionCodeFixProvider : CodeFixProviderBase
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(AnalyzerType.NamingConventionAnalyzerId.ToDiagnosticId());
            }
        }

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