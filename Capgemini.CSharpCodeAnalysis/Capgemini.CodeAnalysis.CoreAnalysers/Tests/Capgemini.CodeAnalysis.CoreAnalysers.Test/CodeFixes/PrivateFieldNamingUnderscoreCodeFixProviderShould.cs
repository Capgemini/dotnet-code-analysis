using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.CodeFixes
{
    [TestClass]
    public class PrivateFieldNamingUnderscoreCodeFixProviderShould : CodeFixVerifier
    {
        [TestMethod]
        public void PrependUnderscoreToPrivateFieldName()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string _fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void PrependUnderscoreToPrivateFieldNamePreservingLeadingTrivia()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            // some leading trivia
            private string fieldThatDoesNotMatch;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            // some leading trivia
            private string _fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void PrependUnderscoreToPrivateFieldNamePreservingTrailingTrivia()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch; // some trailing trivia
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string _fieldThatDoesNotMatch; // some trailing trivia
        }
    }";
            VerifyCSharpFix(original, result);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PrivateFieldNamingUnderscoreAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new PrivateFieldNamingUnderscoreCodeFixProvider();
        }
    }
}
