﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class IfStatementAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void IfStatementWithBraces_Passes()
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
                if(input>variable1)
                {
                    variable2 = ""Maybe not hello world!"";
                }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void IfStatementWithoutBraces_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0012",
                Message = $"{nameof(IfStatementAnalyzer)}: Please ensure that If statements have corresponding curly braces.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
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
                var variable2  = ""Hello world"";
                if(input>variable1)
                    variable2 = ""Maybe not hello world!"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void IfStatementWithBraces_ForIfElseIfAndElseStatements_Passes()
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
                if(input>variable1)
                {    variable2 = ""Maybe not hello world!"";}
                else  if(input<variable1)
                {    variable2 = ""Hello world (not)!"";}
                else
                {    variable2 = ""Hello world!"";}
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new IfStatementAnalyzer();
        }
    }
}