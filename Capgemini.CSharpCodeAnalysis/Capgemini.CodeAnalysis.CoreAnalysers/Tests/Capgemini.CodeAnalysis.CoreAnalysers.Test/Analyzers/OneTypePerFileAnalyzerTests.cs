﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class OneTypePerFileAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void OneClassInaFilePasses()
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
        public void TwoClassesInOneFile_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0011",
                Message = $"{nameof(OneTypePerFileAnalyzer)}: Each file should contain only one type. Please split the types into multiple files.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 9, 15)
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
            public TypeName()
{
}
        }

        public class TypeName2
        {   
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }
        
        [TestMethod]
        public void MultipleClassesAndEnumsInOneFile_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0011",
                Message = $"{nameof(OneTypePerFileAnalyzer)}: Each file should contain only one type. Please split the types into multiple files.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 9, 15)
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
        public enum TestEnum
{
    Value1 = 1,
Value2 = 2
}

        public enum TestEnum
{
    Value1 = 1,
Value2 = 2
}

        public class TypeName
        {   
            public TypeName()
{
}
        }

        public class TypeName2
        {   
            public TypeName()
{
}
        }

    }";

            VerifyCSharpDiagnostic(test, expected);
        }



        [TestMethod]
        public void MultipleClassesInterfacesAndEnumsInOneFile_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0011",
                Message = $"{nameof(OneTypePerFileAnalyzer)}: Each file should contain only one type. Please split the types into multiple files.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 9, 15)
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
        public enum TestEnum
{
    Value1 = 1,
Value2 = 2
}

        public enum TestEnum
{
    Value1 = 1,
Value2 = 2
}

        public class TypeName
        {   
            public TypeName()
{
}
        }

public interface ITest1
{
    void DoStuff();
}
public interface ITest2
{
    void DoStuff();
}

        public class TypeName2
        {   
            public TypeName()
{
}
        }

    }";

            VerifyCSharpDiagnostic(test, expected);
        }



        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new OneTypePerFileAnalyzer();
        }
    }
} 