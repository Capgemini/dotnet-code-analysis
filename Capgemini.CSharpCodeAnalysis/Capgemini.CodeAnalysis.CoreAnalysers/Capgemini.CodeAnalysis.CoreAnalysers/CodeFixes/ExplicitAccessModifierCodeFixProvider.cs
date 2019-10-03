using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    /// <summary>
    /// Implements the Explicit Access Modifier CodeFixProvider.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExplicitAccessModifierCodeFixProvider))]
    [Shared]
    public class ExplicitAccessModifierCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// Gets the overridden FixableDiagnosticIds.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AnalyzerType.ExplicitAccessModifiersAnalyzerId.ToDiagnosticId());

        /// <summary>
        /// Overrides RegisterCodeFixesAsync.
        /// </summary>
        /// <param name="context">An instance of <see cref="CodeFixContext"/> to support the analysis.</param>
        /// <returns>A task that, when completed, will contain the result of the Code Fix registration.</returns>
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var statement = root.FindNode(diagnosticSpan);

            var semanticModel = context.Document.GetSemanticModelAsync().Result;
            var symbol = semanticModel.GetDeclaredSymbol(statement);

            context.RegisterCodeFix(CodeAction.Create("Add public access modifier", x => AddModifier(context.Document, root, statement, Accessibility.Public), "Add public access modifier"), diagnostic);
            context.RegisterCodeFix(CodeAction.Create("Add internal access modifier", x => AddModifier(context.Document, root, statement, Accessibility.Internal), "Add internal access modifier"), diagnostic);
            context.RegisterCodeFix(CodeAction.Create("Add protected access modifier", x => AddModifier(context.Document, root, statement, Accessibility.Protected), "Add protected access modifier"), diagnostic);
        }

        private Task<Solution> AddModifier(Document document, SyntaxNode root, SyntaxNode statement, Accessibility accessibility)
        {
            var generator = SyntaxGenerator.GetGenerator(document);
            System.Console.WriteLine(accessibility);

            var newStatement = generator.WithAccessibility(statement, accessibility);

            var newRoot = root.ReplaceNode(statement, newStatement);
            return Task.FromResult(document.WithSyntaxRoot(newRoot).Project.Solution);
        }
    }
}