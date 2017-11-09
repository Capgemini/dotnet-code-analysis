using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Models
{
    /// <summary>
    /// This class is responsible for generating the diagnostics messages for this analyzer
    /// </summary>
    public class DiagnosticsManager
    {
        /// <summary>
        /// Creates the naming convention diagnostic.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="displayedMessage">The displayed message.</param>
        public void CreateNamingConventionDiagnostic(SyntaxNodeAnalysisContext context, Location location, DiagnosticDescriptor rule, string displayedMessage)
        {
            var diagnostics = Diagnostic.Create(rule, location, displayedMessage);
            context.ReportDiagnostic(diagnostics);
        }

        /// <summary>
        /// Creates the comments diagnostic.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        public void CreateCommentsDiagnostic(SyntaxNodeAnalysisContext context, string name, Location location,
            DiagnosticDescriptor rule)
        {
            var diagnostics = Diagnostic.Create(rule, location, $"{name} does not include valid comments.");
            context.ReportDiagnostic(diagnostics);
        }


        /// <summary>
        /// Creates the explicit access diagnostic.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        public void CreateExplicitAccessDiagnostic(SyntaxNodeAnalysisContext context, string name, Location location, DiagnosticDescriptor rule)
        {
            var diagnostics = Diagnostic.Create(rule, location, $"{name} must include an access modifier.");
            context.ReportDiagnostic(diagnostics);
        }

        public void CreateStaticClassDiagnostic(SyntaxNodeAnalysisContext context, Location location, DiagnosticDescriptor rule)
        {
            var diagnostics = Diagnostic.Create(rule, location, "Static classes must be avoided unless there is no better option!");
            context.ReportDiagnostic(diagnostics);
        }
        public void CreateCommentsTooLongDiagnostic(SyntaxNodeAnalysisContext context, Location location, DiagnosticDescriptor rule, string objectName, int threshold)
        {
            CreateCommentsTooLongDiagnostic(context, location, rule, $"Documentation comments from {objectName} exceed the allowed maximum number of lines {threshold}!");
        }

        public void CreateCommentsTooLongDiagnostic(SyntaxNodeAnalysisContext context, Location location, DiagnosticDescriptor rule, string message)
        {
            var diagnostics = Diagnostic.Create(rule, location, message);
            context.ReportDiagnostic(diagnostics);
        }
        public void CreateHardCodedValueDiagnostic(SyntaxNodeAnalysisContext context, Location location, DiagnosticDescriptor rule, string literalValue)
        {
            var diagnostics = Diagnostic.Create(rule, location, $"Hard-coded values must be avoided at all costs. Declare {literalValue} as a constants or fields as appropriate.");
            context.ReportDiagnostic(diagnostics);
        }
    }
}