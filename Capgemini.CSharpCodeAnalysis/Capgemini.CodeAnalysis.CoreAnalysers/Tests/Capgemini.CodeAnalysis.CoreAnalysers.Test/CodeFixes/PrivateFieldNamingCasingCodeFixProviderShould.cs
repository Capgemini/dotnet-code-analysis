using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.CodeFixes
{
    [TestClass]
    public class PrivateFieldNamingCasingCodeFixProviderShould : CodeFixVerifier
    {
        [TestMethod]
        public void CorrectCaseOfPrivateFieldName()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string FieldThatDoesNotMatch;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void CorrectCaseOfPrivateFieldNameWhenTwoInitialCapitals()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string FIeldThatDoesNotMatch;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void CorrectCaseOfPrivateFieldNamePreservingLeadingTrivia()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            // some leading trivia
            private string FieldThatDoesNotMatch;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            // some leading trivia
            private string fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void CorrectCaseOfPrivateFieldNamePreservingTrailingTrivia()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string FieldThatDoesNotMatch; // some trailing trivia
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch; // some trailing trivia
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void CorrectCaseOfPrivateFieldNameWhenLastLetterIsCapitalized()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string FieldThatDoesNotMatcH;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void CorrectCaseOfPrivateFieldNameWhenMoreThanOneLetterIsCapitalizedInARow()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string FieldTOrename;
        }
    }";

            var result = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldTorename;
        }
    }";
            VerifyCSharpFix(original, result);
        }

        // ToDo - when we reach the appropriate point, this needs to be extracted to enable sharing between renaming analyzers
        [DataTestMethod]
        [DataRow("XML", "fieldWithAcronymOfXmlToUpdate")]
        [DataRow("API", "fieldWithAcronymOfApiToUpdate")]
        [DataRow("CSV", "fieldWithAcronymOfCsvToUpdate")]
        [DataRow("TXT", "fieldWithAcronymOfTxtToUpdate")]
        [DataRow("CPU", "fieldWithAcronymOfCpuToUpdate")]
        [DataRow("FAQ", "fieldWithAcronymOfFaqToUpdate")]
        [DataRow("GPU", "fieldWithAcronymOfGpuToUpdate")]
        [DataRow("IDE", "fieldWithAcronymOfIdeToUpdate")]
        [DataRow("PDF", "fieldWithAcronymOfPdfToUpdate")]
        [DataRow("AJAX", "fieldWithAcronymOfAjaxToUpdate")]
        [DataRow("ACK", "fieldWithAcronymOfAckToUpdate")]
        [DataRow("AES", "fieldWithAcronymOfAesToUpdate")]
        [DataRow("DES", "fieldWithAcronymOfDesToUpdate")]
        [DataRow("CAD", "fieldWithAcronymOfCadToUpdate")]
        [DataRow("CDN", "fieldWithAcronymOfCdnToUpdate")]
        [DataRow("COM", "fieldWithAcronymOfComToUpdate")]
        [DataRow("CRC", "fieldWithAcronymOfCrcToUpdate")]
        [DataRow("CSS", "fieldWithAcronymOfCssToUpdate")]
        [DataRow("HTML", "fieldWithAcronymOfHtmlToUpdate")]
        [DataRow("HTTP", "fieldWithAcronymOfHttpToUpdate")]
        [DataRow("TCPIP", "fieldWithAcronymOfTcpIpToUpdate")]
        [DataRow("TCP", "fieldWithAcronymOfTcpToUpdate")]
        [DataRow("ADO", "fieldWithAcronymOfAdoToUpdate")]
        [DataRow("DB", "fieldWithAcronymOfDbToUpdate")]
        [DataRow("DVD", "fieldWithAcronymOfDvdToUpdate")]
        [DataRow("DSN", "fieldWithAcronymOfDsnToUpdate")]
        [DataRow("DW", "fieldWithAcronymOfDwToUpdate")]
        [DataRow("EMAIL", "fieldWithAcronymOfEmailToUpdate")]
        [DataRow("EMail", "fieldWithAcronymOfEmailToUpdate")]
        [DataRow("FQDN", "fieldWithAcronymOfFqdnToUpdate")]
        [DataRow("GUI", "fieldWithAcronymOfGuiToUpdate")]
        [DataRow("IOT", "fieldWithAcronymOfIoTtoUpdate")]
        [DataRow("JPG", "fieldWithAcronymOfJpgToUpdate")]
        [DataRow("JPEG", "fieldWithAcronymOfJpegToUpdate")]
        [DataRow("BMP", "fieldWithAcronymOfBmpToUpdate")]
        [DataRow("XLS", "fieldWithAcronymOfXlsToUpdate")]
        [DataRow("MP", "fieldWithAcronymOfMpToUpdate")]
        [DataRow("DOC", "fieldWithAcronymOfDocToUpdate")]
        [DataRow("DOCX", "fieldWithAcronymOfDocxToUpdate")]
        [DataRow("XLSX", "fieldWithAcronymOfXlsxToUpdate")]
        [DataRow("PPT", "fieldWithAcronymOfPptToUpdate")]
        [DataRow("PPTX", "fieldWithAcronymOfPptxToUpdate")]
        [DataRow("LOG", "fieldWithAcronymOfLogToUpdate")]
        [DataRow("MSG", "fieldWithAcronymOfMsgToUpdate")]
        [DataRow("RTF", "fieldWithAcronymOfRtfToUpdate")]
        [DataRow("DAT", "fieldWithAcronymOfDatToUpdate")]
        [DataRow("PPS", "fieldWithAcronymOfPpsToUpdate")]
        [DataRow("WAV", "fieldWithAcronymOfWavToUpdate")]
        [DataRow("WMA", "fieldWithAcronymOfWmaToUpdate")]
        [DataRow("AVI", "fieldWithAcronymOfAviToUpdate")]
        [DataRow("GIF", "fieldWithAcronymOfGifToUpdate")]
        [DataRow("PNG", "fieldWithAcronymOfPngToUpdate")]
        [DataRow("TIFF", "fieldWithAcronymOfTiffToUpdate")]
        [DataRow("TIF", "fieldWithAcronymOfTifToUpdate")]
        [DataRow("PS", "fieldWithAcronymOfPsToUpdate")]
        [DataRow("JS", "fieldWithAcronymOfJsToUpdate")]
        [DataRow("PHP", "fieldWithAcronymOfPhpToUpdate")]
        [DataRow("JSP", "fieldWithAcronymOfJspToUpdate")]
        [DataRow("DLL", "fieldWithAcronymOfDllToUpdate")]
        [DataRow("ICO", "fieldWithAcronymOfIcoToUpdate")]
        [DataRow("RAR", "fieldWithAcronymOfRarToUpdate")]
        [DataRow("ZIP", "fieldWithAcronymOfZipToUpdate")]
        [DataRow("BIN", "fieldWithAcronymOfBinToUpdate")]
        [DataRow("BAK", "fieldWithAcronymOfBakToUpdate")]
        [DataRow("TMP", "fieldWithAcronymOfTmpToUpdate")]
        public void CorrectCaseOfPrivateFieldNameWhenOriginalNameContainsKnownAcronym(string acronym, string expectedNewFieldName)
        {
            var original = $@"
    using System;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
            private string FieldWithAcronymOf{acronym}ToUpdate;
        }}
    }}";

            var result = $@"
    using System;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
            private string {expectedNewFieldName};
        }}
    }}";
            VerifyCSharpFix(original, result);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PrivateFieldNamingCasingAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new PrivateFieldNamingCasingCodeFixProvider();
        }
    }
}
