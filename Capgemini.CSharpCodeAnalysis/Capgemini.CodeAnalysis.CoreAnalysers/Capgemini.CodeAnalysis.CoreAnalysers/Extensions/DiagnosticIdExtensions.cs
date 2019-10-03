using Capgemini.CodeAnalysis.CoreAnalysers.Models;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Extensions
{
    /// <summary>
    /// Implements bespoke extensions for DiagnosticId.
    /// </summary>
    public static class DiagnosticIdExtensions
    {
        /// <summary>
        /// Extends  AnalyzerType with the method ToDiagnosticId.
        /// </summary>
        /// <param name="diagnosticId">The AnalyzerType to convert to the required string.</param>
        /// <returns>The diagnostic Id formatted as required.</returns>
        public static string ToDiagnosticId(this AnalyzerType diagnosticId)
        {
            return $"CAP{(int)diagnosticId:D4}";
        }
    }
}