using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
{
    [TestClass]
    public class XmlCommentsAnalyserTests : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new XmlCommentsAnalyzer();
        }

        [TestMethod]
        public void XmlCommentsAnalyserTest_Passes()
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
        /// <summary>
        /// test
        /// </summary>
        public class ClassName
        {   

            static ClassName()
            {
            }

        /// <summary>
        /// test
        /// </summary>
            public void Test()
            {
            }
        }
    }";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void XmlCommentsAnalyserTest_ClassCommentMissing()
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
        public class TypeName
        {   


            static TypeName()
            {
            }

        /// <summary>
        /// test
        /// </summary>
            public void Test()
            {
            }
        }
    }";


            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.XmlCommentsAnalyzerId,
                Message = $"{nameof(XmlCommentsAnalyzer)} 'TypeName does not include valid comments.'",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                          new[] {
                                    new DiagnosticResultLocation("Test0.cs", 11, 22)
                              }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

    }
}
