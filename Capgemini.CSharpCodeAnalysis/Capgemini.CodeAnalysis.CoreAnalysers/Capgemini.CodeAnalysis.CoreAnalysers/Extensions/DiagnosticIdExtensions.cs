using Capgemini.CodeAnalysis.CoreAnalysers.Models;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Extensions
{
    public static class DiagnosticIdExtensions
    {
        public static string ToDiagnosticId(this AnalyzerType diagnosticId) => $"CAP{(int)diagnosticId:D4}";
    }
}