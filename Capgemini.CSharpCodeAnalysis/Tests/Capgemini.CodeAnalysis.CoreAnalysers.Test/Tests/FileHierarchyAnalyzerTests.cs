using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
{
    [TestClass]
    public class FileHierarchyAnalyzerTests : CodeFixVerifier
    {

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new FileHierarchyAnalyzer();
        }
    }
}