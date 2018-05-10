namespace Capgemini.CodeAnalysis.Foundation
{
    public static class DiagnosticIdExtensions
    {
        public static string ToDiagnosticId(this AnalyzerType diagnosticId) => $"CAP{(int)diagnosticId:D4}";
    }
}