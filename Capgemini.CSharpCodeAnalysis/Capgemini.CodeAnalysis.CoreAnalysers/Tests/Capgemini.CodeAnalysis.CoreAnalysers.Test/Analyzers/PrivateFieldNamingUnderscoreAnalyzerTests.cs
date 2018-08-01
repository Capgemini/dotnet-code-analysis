using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{

    [TestClass]
    public class PrivateFieldNamingUnderscoreAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForNoPrivateFields()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   

        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForPrivateField()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "CAP0017",
                Message = $"{nameof(PrivateFieldNamingUnderscoreAnalyzer)}: Field 'fieldThatDoesNotMatch' does not start with '_'",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 28)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AnalysisPassesForPrivateFieldWithLeadingTrivia()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            // some leading trivia
            private string fieldThatDoesNotMatch;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "CAP0017",
                Message = $"{nameof(PrivateFieldNamingUnderscoreAnalyzer)}: Field 'fieldThatDoesNotMatch' does not start with '_'",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 28)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PrivateFieldNamingUnderscoreAnalyzer();
        }
    }
}
