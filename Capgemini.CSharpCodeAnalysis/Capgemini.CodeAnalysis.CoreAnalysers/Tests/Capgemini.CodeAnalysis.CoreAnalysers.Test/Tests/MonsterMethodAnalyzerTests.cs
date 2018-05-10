using System;
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
    public class MonsterMethodAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodWithLessThan80LinesOfExecutableCode_Passes()
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
        public class TypeName
        {{   
            public void DoStuff()
            {{
                {GenerateRandomIntVariableString(40)}
                {GenerateRandomStringVariableString(39)}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void MethodWithLess80LinesOfExecutableCode_Passes()
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
        public class TypeName
        {{   
            public void DoStuff()
            {{
                {GenerateRandomIntVariableString(40)}
                {GenerateRandomStringVariableString(40)}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void MethodWithMoreThan80LinesOfExecutableCode_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.MonsterMethodAnalyzerId,
                Message = $"{nameof(MonsterMethodAnalyzer)}: This method is longer than 80 lines of executable code. Please consider splitting this method into smaller methods.",
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
            public void DoStuff()
            {{
                {GenerateRandomIntVariableString(40)}
                {GenerateRandomStringVariableString(41)}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test,expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MonsterMethodAnalyzer();
        }

        private string GenerateRandomIntVariableString(int numberOfVariables)
        {
            var sb = new StringBuilder();
            for (var counter = 0; counter < numberOfVariables; counter++)
            {
                sb.AppendLine($"var intVarible{counter} = {counter}");
            }
            return sb.ToString();
        }
        private string GenerateRandomStringVariableString(int numberOfVariables)
        {
            var sb = new StringBuilder();
            for (var counter = 0; counter < numberOfVariables; counter++)
            {
                sb.AppendLine($"var stringVarible{counter} = \"{DateTime.UtcNow:F}\"");
            }
            return sb.ToString();
        }
    }
}