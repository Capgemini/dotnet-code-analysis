using System.Text;
using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
{
    [TestClass]
    public class LargeCommentedCodeAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void NoCommentsInCode_Passes()
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
        public void MethodWithCommentsMoreThan20Lines_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0005",
                Message = $"{nameof(LargeCommentedCodeAnalyzer)}: These lines of code are redundant. Please delete them.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 13, 25)
                    }
            };

            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {{
        public class TypeName
        {{  
/*
{GenerateRandomText(21)}
*/ 
            public void DoStuff(int input)
            {{               
                var variable1 = 20;
                var variable2  = ""Hello world"";
                if(input>variable1)
                {{
                    variable2 = ""Maybe not hello world!"";
                }}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }


        [TestMethod]
        public void ClassWithCommentsLessThan20Lines_Passes()
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {{
        /*{GenerateRandomText(18)}*/
        public class TypeName
        {{   
            public void DoStuff(int input)
            {{
                var variable1 = 20;
                var variable2  = ""Hello world"";
                if(input>variable1)
                {{
                    variable2 = ""Maybe not hello world!"";
                }}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassWithComments20Lines_Passes()
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {{
        {GenerateClassSummary(18)}        
        public class TypeName
        {{   
            public void DoStuff(int input)
            {{
                var variable1 = 20;
                var variable2  = ""Hello world"";
                if(input>variable1)
                {{
                    variable2 = ""Maybe not hello world!"";
                }}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        private string GenerateRandomText(int noOfLines)
        {
            var sb = new StringBuilder();
            for (var counter = 0; counter < noOfLines; counter++)
            {
                sb.AppendLine($"Some random text {counter}");
            }
            return sb.ToString();
        }

        private string GenerateClassSummary(int noOfLines)
        {
            var sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            for (var counter = 0; counter < noOfLines; counter++)
            {
                sb.AppendLine($"/// Some random text {counter}");
            }
            sb.AppendLine("/// </summary>");
            return sb.ToString();
        }


        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new LargeCommentedCodeAnalyzer();
        }
    }
}