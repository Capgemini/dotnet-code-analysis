﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Rename;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    /// <summary>
    /// Implements the Private Field Naming CasingCodeFixProvider.
    /// All tests have been removed as, now this is deprecated, they fail and changes are not supported - deprecated ;-).
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PrivateFieldNamingCasingCodeFixProvider))]
    [Shared]
    public class PrivateFieldNamingCasingCodeFixProvider : CodeFixProviderBase
    {
        /// <summary>
        /// Gets the overridden FixableDiagnosticIds.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId());

        /// <summary>
        /// Overrides RegisterCodeFixesAsync.
        /// </summary>
        /// <param name="context">An instance of <see cref="CodeFixContext"/> to support the analysis.</param>
        /// <returns>A task that, when completed, will contain the result of the Code Fix registration.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var contextRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var firstDiagnostic = context.Diagnostics.First();
            var privateField = contextRoot.FindToken(firstDiagnostic.Location.SourceSpan.Start);
            context.RegisterCodeFix(
                                    CodeAction.Create(
                                        $"Change name of '{privateField}'",
                                        cancellationToken => UpdateFieldNameCasing(context.Document, privateField, cancellationToken),
                                        AnalyzerType.PrivateFieldNamingUnderscoreAnalyzerId.ToDiagnosticId()),
                                    firstDiagnostic);
        }

        private static string RemoveConsecutiveCapitals(string namePostAcronymUpdates)
        {
            var stringBuilder = new StringBuilder();
            var previousLetterWasCapitalized = false;
            stringBuilder.Append(namePostAcronymUpdates.Substring(0, 2));
            for (var i = 2; i < namePostAcronymUpdates.Length - 1; i++)
            {
                if (char.IsUpper(namePostAcronymUpdates[i]) && previousLetterWasCapitalized)
                {
#pragma warning disable CA1308 // Normalize strings to uppercase - lowercase is required here
                    stringBuilder.Append(namePostAcronymUpdates[i].ToString().ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
                    previousLetterWasCapitalized = false;
                }
                else if (char.IsUpper(namePostAcronymUpdates[i]))
                {
                    stringBuilder.Append(namePostAcronymUpdates[i]);
                    previousLetterWasCapitalized = true;
                }
                else
                {
                    stringBuilder.Append(namePostAcronymUpdates[i]);
                    previousLetterWasCapitalized = false;
                }
            }

            stringBuilder.Append(namePostAcronymUpdates.Substring(namePostAcronymUpdates.Length - 1));

            return stringBuilder.ToString();
        }

        private static bool NameHasConsecutiveCapitalLetters(string namePostAcronymUpdates)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(namePostAcronymUpdates, "([A-Z]){2}");
        }

        // ToDo - when we reach the appropriate point, this needs to be extracted to enable sharing between renaming analyzers
        private static string UpdateForKnownAcronyms(string currentName)
        {
            var acronyms = new Dictionary<string, string>
            {
                { "PPTX", "Pptx" },
                { "PPT", "Ppt" },
                { "XML", "Xml" },
                { "API", "Api" },
                { "CSV", "Csv" },
                { "TXT", "Txt" },
                { "CPU", "Cpu" },
                { "FAQ", "Faq" },
                { "GPU", "Gpu" },
                { "IDE", "Ide" },
                { "PDF", "Pdf" },
                { "AJAX", "Ajax" },
                { "ACK", "Ack" },
                { "AES", "Aes" },
                { "DES", "Des" },
                { "CAD", "Cad" },
                { "CDN", "Cdn" },
                { "COM", "Com" },
                { "CRC", "Crc" },
                { "CSS", "Css" },
                { "HTML", "Html" },
                { "HTTP", "Http" },
                { "TCPIP", "TcpIp" },
                { "TCP", "Tcp" },
                { "ADO", "Ado" },
                { "DB", "Db" },
                { "DVD", "Dvd" },
                { "DSN", "Dsn" },
                { "DW", "Dw" },
                { "EMAIL", "Email" },
                { "EMail", "Email" },
                { "FQDN", "Fqdn" },
                { "GUI", "Gui" },
                { "IOT", "IoT" },
                { "JPG", "Jpg" },
                { "JPEG", "Jpeg" },
                { "BMP", "Bmp" },
                { "XLSX", "Xlsx" },
                { "XLS", "Xls" },
                { "MP", "Mp" },
                { "DOCX", "Docx" },
                { "DOC", "Doc" },
                { "LOG", "Log" },
                { "MSG", "Msg" },
                { "RTF", "Rtf" },
                { "DAT", "Dat" },
                { "PPS", "Pps" },
                { "WAV", "Wav" },
                { "WMA", "Wma" },
                { "AVI", "Avi" },
                { "GIF", "Gif" },
                { "PNG", "Png" },
                { "TIFF", "Tiff" },
                { "TIF", "Tif" },
                { "PS", "Ps" },
                { "JSP", "Jsp" },
                { "JS", "Js" },
                { "PHP", "Php" },
                { "DLL", "Dll" },
                { "ICO", "Ico" },
                { "RAR", "Rar" },
                { "ZIP", "Zip" },
                { "BIN", "Bin" },
                { "BAK", "Bak" },
                { "TMP", "Tmp" }
            };
            var updatedName = currentName;
            foreach (var acronym in acronyms)
            {
                updatedName = updatedName.Replace(acronym.Key, acronym.Value);
            }

            return updatedName;
        }

        private static string UpdateInitialPartsOfTheName(string initialUpdates)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            return initialUpdates.Substring(0, 2).ToLowerInvariant() + initialUpdates.Substring(2, initialUpdates.Length - 3) + initialUpdates.Substring(initialUpdates.Length - 1).ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        private static string ChangeCase(string currentName)
        {
            var initialUpdateForCurrentName = UpdateInitialPartsOfTheName(currentName);
            var namePostAcronymUpdates = UpdateForKnownAcronyms(initialUpdateForCurrentName);
            System.Console.WriteLine(namePostAcronymUpdates);
            if (!NameHasConsecutiveCapitalLetters(namePostAcronymUpdates))
            {
                return namePostAcronymUpdates;
            }

            return RemoveConsecutiveCapitals(namePostAcronymUpdates);
        }

        private async Task<Solution> UpdateFieldNameCasing(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            var currentName = declaration.ValueText;
            var newName = ChangeCase(currentName);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            var solution = document.Project.Solution;

            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }
    }
}