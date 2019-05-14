using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Test.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class MethodParametersAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForNoDefinedConstructor()
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
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForEmptyConstructor()
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
            public void MethodName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForMethodWithNumberOfParamatersLessThanWarningLevel()
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
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForMethodWithNumberOfParamatersEqualToWarningLevel()
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
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisWarnsForMethodWithNumberOfParamatersGreaterThanWarningLevelButLessThanErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0016",
                Message =
                    "MethodParametersAnalyzer: Method has a total of 6 Parameters which is greater-than the recommended maximum of 5. Please consider refactoring the method / class.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 25)
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
        public class TypeName
        {   
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AnalysisFailsForMethodWithNumberOfParamatersEqualToTheErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0016",
                Message =
                    "MethodParametersAnalyzer: Method has a total of 10 Parameters which is equal to the recommended maximum of 10. Please refactor the method / class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 12, 25)
                    }
            };

            var test = @"using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10)
{
}
        }
    }
    ";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AnalysisFailsForMethodWithNumberOfParamatersGreaterThanErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0016",
                Message =
                    "MethodParametersAnalyzer: Method has a total of 11 Parameters which is greater-than the recommended maximum of 10. Please refactor the method / class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 25)
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
        public class TypeName
        {   
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string parameter11)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

   
        [TestMethod]
        public void IgnoreGneratedSourceCode()
        { 
            var test = CommonConstants.AutoGeneratedCodeHeader + @"
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
            public void MethodName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string parameter11)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

       protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MethodParametersAnalyzer();
        }
    }
}