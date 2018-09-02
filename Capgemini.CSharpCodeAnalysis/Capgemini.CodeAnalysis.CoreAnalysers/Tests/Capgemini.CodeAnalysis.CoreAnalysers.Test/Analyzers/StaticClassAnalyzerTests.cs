﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class StaticClassAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void NonStaticClassPasses()
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
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void NonStaticClass_WithStaticMethod_Passes()
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
            public TypeName()
{
}

            public void DoStuff()
{
}
            public static void DoStuff2()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void StaticClass_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0003",
                Message = $"{nameof(StaticClassAnalyzer)}: Static classes must be avoided unless there is no better option.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 11, 29)
                    }
            };

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public static class TypeName
        {   

        } 
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void StaticClassWithExtensionMethod_Passes()
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
        public static class TypeName
        {   
            public static void TypeName(this string input)
{
}
        } 
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new StaticClassAnalyzer();
        }
    }
}