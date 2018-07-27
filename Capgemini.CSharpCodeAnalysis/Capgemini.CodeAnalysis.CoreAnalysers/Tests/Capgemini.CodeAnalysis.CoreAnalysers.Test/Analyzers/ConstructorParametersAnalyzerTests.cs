using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class ConstructorParametersAnalyzerTests : CodeFixVerifier
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
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForConstructorWithNumberOfParamatersLessThanWarningLevel()
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
            public TypeName(string parameter1, string parameter2, string parameter3, string parameter4)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisPassesForConstructorWithNumberOfParamatersEqualToWarningLevel()
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
            public TypeName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AnalysisWarnsForConstructorWithNumberOfParamatersGreaterThanWarningLevelButLessThanErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0015",
                Message =
                    "ConstructorParametersAnalyzer: Constructor has a total of 6 Parameters which is greater-than the recommended maximum of 5. Please consider refactoring the constructor / class.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 20)
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
            public TypeName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AnalysisFailsForConstructorWithNumberOfParamatersEqualToTheErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0015",
                Message =
                    "ConstructorParametersAnalyzer: Constructor has a total of 10 Parameters which is equal to the recommended maximum of 10. Please refactor the constructor / class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 20)
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
            public TypeName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void AnalysisFailsForConstructorWithNumberOfParamatersGreaterThanErrorLevel()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0015",
                Message =
                    "ConstructorParametersAnalyzer: Constructor has a total of 11 Parameters which is greater-than the recommended maximum of 10. Please refactor the constructor / class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 20)
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
            public TypeName(string parameter1, string parameter2, string parameter3, string parameter4, string parameter5, string parameter6, string parameter7, string parameter8, string parameter9, string parameter10, string parameter11)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ConstructorParametersAnalyzer();
        }
    }
}