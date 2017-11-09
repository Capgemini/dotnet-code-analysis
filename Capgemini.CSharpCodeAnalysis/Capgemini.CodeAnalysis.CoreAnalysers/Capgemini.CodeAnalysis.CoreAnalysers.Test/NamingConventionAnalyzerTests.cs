using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test
{
    [TestClass]
    public class NamingConventionAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void NamingConventionAnalyzer_Passes_ClassName()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void NamingConventionAnalyzer_Fails_ClassNameEnsWithUpperCharacter()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeNameG
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.NamingConventionAnalyzerId,
                Message = $"{nameof(NamingConventionAnalyzer)}: TypeNameG does not satisfy naming convention. \nTypeNameG must start with one upper case character, \nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void NamingConventionAnalyzer_Passes_PrivateFieldNameMustStartWithUnderscore()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string _field1;
        }
    }";
            
            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void NamingConventionAnalyzer_Fails_PrivateFieldNameMustStartWithUnderscore()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string Field1;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.NamingConventionAnalyzerId,
                Message = $"{nameof(NamingConventionAnalyzer)}: Field1 does not satisfy naming convention. \nField1 must start with underscore character followed by atleast two lower case characters, \nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 13, 28)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NamingConventionAnalyzer();
        }
    }
}