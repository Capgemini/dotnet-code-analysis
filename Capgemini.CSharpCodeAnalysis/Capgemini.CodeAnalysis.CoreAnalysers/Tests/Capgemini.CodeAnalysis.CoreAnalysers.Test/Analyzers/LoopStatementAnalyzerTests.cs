﻿using System;
using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Test.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class LoopStatementAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = string.Empty;

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ForStatementWithBracesPasses()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {   
            public void DoStuff(int input)
            {
                var variable1 = 20;
                var variable2  = ""Hello world"";
                for(var counter=0; counter < variable1; counter++)
                {
                    variable2 += counter;
                }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ForStatementWithoutBracesFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0013",
                Message = $"{nameof(LoopStatementAnalyzer)}: Please ensure that for statements have corresponding curly braces.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 17)
                    }
            };

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {   
            public void DoStuff(int input)
            {
                var variable1 = 20;
                var variable2  = 5;
                for(var counter=0; counter < variable1; counter++)
                    variable2 += counter;
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void IgnoresGeneratedSourceCode()
        {
            var test = CommonConstants.AutoGeneratedCodeHeader + @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {   
            public void DoStuff(int input)
            {
                var variable1 = 20;
                var variable2  = 5;
                for(var counter=0; counter < variable1; counter++)
                    variable2 += counter;
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ThrowArgumentNullExceptionWhenContextNotSupplied()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new LoopStatementAnalyzer().Initialize(null)).Message.Equals("An instance of LoopStatementAnalyzer was not supplied.", StringComparison.Ordinal);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new LoopStatementAnalyzer();
        }
    }
}