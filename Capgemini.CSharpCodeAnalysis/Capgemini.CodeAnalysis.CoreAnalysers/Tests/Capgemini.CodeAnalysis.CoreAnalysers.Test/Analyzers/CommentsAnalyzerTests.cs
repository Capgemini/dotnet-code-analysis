using System;
using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Test.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class CommentsAnalyzerTests : CodeFixVerifier
    {
        private readonly int threshold = 30;

        [TestMethod]
        public void MethodDeclarationNoCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclarationSummaryTagsCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public void Method1()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclarationLessThan30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclaration30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclarationMore30LinesCommentsFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from Method1 exceed the allowed maximum number of lines {threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 38, 25)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void PropertyDeclarationNoCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public string GetTypeName{get;set;}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclarationSummaryTagsCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public string GetTypeName{get;set;}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclarationLessThan30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclaration30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclarationMore30LinesCommentsFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from GetTypeName exceed the allowed maximum number of lines {threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 38, 27)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void ClassDeclarationNoCommentsPasses()
        {
            var test = @"

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
        public void ClassDeclarationSummaryTagsCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        /// <summary>
        /// </summary>
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
        public void ClassDeclarationLessThan30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Class comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassDeclaration30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassDeclarationMore30LinesCommentsFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from TypeName exceed the allowed maximum number of lines {threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 36, 22)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void InterfaceDeclarationNoCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public interface ITypeName
        {   
            
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclarationSummaryTagsCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        /// <summary>
        /// </summary>
        public interface ITypeName
        {  
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclarationLessThan30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{   
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclaration30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{ 
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclarationMore30LinesCommentsFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from ITypeName exceed the allowed maximum number of lines {threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 36, 26)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{  
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclarationNoCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclarationLessThan5LinesCommentsBeforeVariablePasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration5LinesCommentsBeforeVariablePasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(5, "// Method1 comments")}
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclarationLessThan5LinesCommentsAfterVariablePasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
                string testString =""TestString"";
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration5LinesCommentsAfterVariablePasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
                string testString =""TestString"";
{GenerateMultipleLinesOfText(5, "// Method1 comments")}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclarationCommentsBeforeAndAfterVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has both leading and trailing comments. Only one type is allowed.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 31, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
                string testString =""TestString""; // Method1 comments
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclarationMultiLineAndSingleLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has both multiline and single line comments. Please use only one type of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 24)
                    }
            };

            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                /*
                   Multiline comments
                */
                //Single line comment
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclarationMultipleMultiLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has multiple MultiLines comments. Please use no more than 1 MultiLines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                /*
                   Multiline comments
                */
                /*
                   Multiline comments
                */               
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclarationMoreThan5LinesOfMultiLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{ 
                /*
                   {GenerateMultipleLinesOfText(6, "Sample comments")}
                */            
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclarationMoreThan5SingleLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{
{GenerateMultipleLinesOfText(6, "// Single line comments")}             
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration6LinesOfMultiLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 16, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{ 
                /*
                   {GenerateMultipleLinesOfText(5, "Sample comments")}
                */            
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration6SingleLineCommentsBeforeVariableFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{
{GenerateMultipleLinesOfText(6, "// Single line comments")}             
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void ConstructorDeclarationNoMethodCommentsPasses()
        {
            var test = @"

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
        public void ConstructorDeclarationWithSummaryMethodCommentsSummaryTagsCommentsPasses()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclarationWithSummaryMethodCommentsLessThan30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclarationWithSummaryMethodComments30LinesCommentsPasses()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclarationWithSummaryMethodCommentsMore30LinesCommentsFails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from TypeName exceed the allowed maximum number of lines {threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 38, 20)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void IgnoresGeneratedSourceCode()
        {
            var test = CommonConstants.AutoGeneratedCodeHeader + $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ThrowArgumentNullExceptionWhenContextNotSupplied()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CommentsAnalyzer().Initialize(null)).Message.Equals("An instance of CommentsAnalyzer was not supplied.", StringComparison.Ordinal);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CommentsAnalyzer();
        }
    }
}