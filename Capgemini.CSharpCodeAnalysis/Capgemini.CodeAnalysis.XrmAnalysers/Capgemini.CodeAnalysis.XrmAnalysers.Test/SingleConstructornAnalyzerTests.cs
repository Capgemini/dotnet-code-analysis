using Capgemini.CodeAnalysis.Foundation;
using Capgemini.CodeAnalysis.XrmAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.XrmAnalysers.Test
{
    [TestClass]
    public class SingleConstructornAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void SingleConstructornAnalyzer_Passes_Class_WithOneConstructor()
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
        public void SingleConstructornAnalyzer_Passes_Class_WithOneConstructorAndNoInjectableAttribute()
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
        [SomeAttribute]
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
        public void SingleConstructornAnalyzer_Passes_Class_WithNoConstructor()
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
        public void SingleConstructornAnalyzer_FailsForClass_MarkedWithInjectable_AndHas_MoreThanOneConstructors()
        {
            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.SingleConstructornAnalyzerId,
                Message = $"{nameof(SingleConstructornAnalyzer)}: TypeName already has a constructor. Only one constructor is allowed for this class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                            new[] {
                                    new DiagnosticResultLocation("Test0.cs", 12, 22)
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
        [Injectable, SomeAttribute]
        public class TypeName
        {  
            public TypeName()
{
}  
            public TypeName(string input)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void SingleConstructornAnalyzer_FailsForClass_MarkedWithInjectableAttribute_AndHas_MoreThanOneConstructors()
        {
            var expected = new DiagnosticResult
            {
                Id = AnalyserConstants.SingleConstructornAnalyzerId,
                Message = $"{nameof(SingleConstructornAnalyzer)}: TypeName already has a constructor. Only one constructor is allowed for this class.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 12, 22)
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
        [InjectableAttribute]
        public class TypeName
        {  
            public TypeName()
{
}  
            public TypeName(string input)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void SingleConstructornAnalyzer_PassesForClass_Without_InjectableAttribute_ButWithMoreThanOneConstructor()
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
            public TypeName(string input)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void SingleConstructornAnalyzer_PassesForClass_Without_InjectableAttribute_HasOtherAttribute_ButWithMoreThanOneConstructor()
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
        [SomeAttributes]
        public class TypeName
        {  
            public TypeName()
{
}  
            public TypeName(string input)
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SingleConstructornAnalyzer();
        }
    }
}