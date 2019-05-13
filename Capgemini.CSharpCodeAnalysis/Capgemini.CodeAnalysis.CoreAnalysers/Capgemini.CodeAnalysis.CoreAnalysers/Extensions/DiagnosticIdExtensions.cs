using Capgemini.CodeAnalysis.CoreAnalysers.Models;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Extensions
{
        /// <summary>
    /// Implements bespoke extensions for  DiagnosticId
    /// </summary>
    public static class DiagnosticIdExtensions
    {
        /// <summary>
        /// Extends  AnalyzerType with the method ToDiagnosticId
        /// </summary>
        /// <param name="diagnosticId"></param>
        /// <returns></returns>
        public static string ToDiagnosticId(this AnalyzerType diagnosticId) => $"CAP{(int)diagnosticId:D4}";
    }
}