using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Models
{
    /// <summary>
    /// This class is responsible for manipulating code comments.
    /// </summary>
    public sealed class CommentsManager
    {
        /// <summary>
        /// Determine if a node has valid comments.
        /// </summary>
        /// <param name="node">An instance of <see cref="SyntaxNode"/> to determine whether comments exist for.</param>
        /// <returns><c>true</c> if valid comments are detected, otherwise <c>false</c>.</returns>
        public static bool HasValidComments(SyntaxNode node)
        {
            var result = HasValidSummaryComments(node);

            if (!result)
            {
                // now let's test for other types of comments
                var comments = ExtractLeadingComments(node);

                result = ProcessComments(comments);
            }

            return result;
        }

        /// <summary>
        /// Determine if a field has valid comments.
        /// </summary>
        /// <param name="node">An instance of <see cref="SyntaxNode"/> to determine whether the comments are deemed valid.</param>
        /// <returns><c>true</c> if valid comments are detected, otherwise <c>false</c>.</returns>
        public static bool FieldHasValidComments(SyntaxNode node)
        {
            var result = HasValidSummaryComments(node);

            if (!result)
            {
                // now let's test for other types of comments
                var comments = ExtractLeadingComments(node);
                comments.AddRange(ExtractTrailingComments(node));

                result = ProcessComments(comments);
            }

            return result;
        }

        /// <summary>
        /// Extracts the documentation comment.
        /// </summary>
        /// <param name="node">An instance of <see cref="SyntaxNode"/> to extract the comments from.</param>
        /// <returns>The appropriate instance of <see cref="DocumentationCommentTriviaSyntax"/> or null if no match located.</returns>
        public static DocumentationCommentTriviaSyntax ExtractDocumentationComment(SyntaxNode node)
        {
            DocumentationCommentTriviaSyntax documentation = null;
            if (node != null && node.HasStructuredTrivia)
            {
                documentation = node.GetLeadingTrivia()
                                    .Select(i => i.GetStructure())
                                    .OfType<DocumentationCommentTriviaSyntax>()
                                    .FirstOrDefault();
            }

            return documentation;
        }

        /// <summary>
        /// Extract Comment.
        /// </summary>
        /// <param name="node">An instance of <see cref="SyntaxNode"/> to extract the comments from.</param>
        /// <returns>A populated <see cref="List{SyntaxTrivia}"/> containing the leading comments.</returns>
        public static List<SyntaxTrivia> ExtractComment(SyntaxNode node)
        {
            var result = ExtractLeadingComments(node);

            return result;
        }

        /// <summary>
        /// Retrieve MultiLine Comments.
        /// </summary>
        /// <param name="node">An instance of <see cref="SyntaxNode"/> to retrieve the comments from.</param>
        /// <returns>A populated <see cref="List{SyntaxTrivia}"/> containing the multi-line comments.</returns>
        public static List<SyntaxTrivia> RetrieveMultiLineComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments.AddRange(node.GetLeadingTrivia()
                        .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia))
                        .ToList());
                comments.AddRange(node.GetTrailingTrivia()
                        .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia))
                        .ToList());
            }

            return comments;
        }

        /// <summary>
        /// Determines whether [has valid summary comments] [the specified node].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if [has valid summary comments] [the specified node]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidSummaryComments(SyntaxNode node)
        {
            var result = false;

            var summary = GetSummaryComment(node);
            if (summary != null)
            {
                var s = summary.Content.ToFullString()?.Replace("///", string.Empty);
                result = !string.IsNullOrWhiteSpace(s);
            }

            return result;
        }

        /// <summary>
        /// Gets the summary comment.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>A populated instance of <see cref="XmlElementSyntax"/>.</returns>
        public static XmlElementSyntax GetSummaryComment(SyntaxNode node)
        {
            return ExtractSummaryComment(node);
        }

        private static bool ProcessComments(List<SyntaxTrivia> comments)
        {
            var result = false;
            if (comments.Count == 1)
            {
                var comment = comments[0];
                var content = comment.ToString().Replace(" ", string.Empty).Trim();
                if (!(content == "//" || content == "/**/"))
                {
                    result = true;
                }
            }

            return result;
        }

        private static List<SyntaxTrivia> ExtractLeadingComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments = node.GetLeadingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                       .ToList();
            }

            return comments;
        }

        private static List<SyntaxTrivia> ExtractTrailingComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments = node.GetTrailingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) || a.IsKind(SyntaxKind.SingleLineCommentTrivia))
                       .ToList();
            }

            return comments;
        }

        private static XmlElementSyntax ExtractSummaryComment(SyntaxNode node)
        {
            XmlElementSyntax summary = null;
            if (node != null && node.HasStructuredTrivia)
            {
                var docummentation = ExtractDocumentationComment(node);
                summary = docummentation?.ChildNodes()
                                       .OfType<XmlElementSyntax>()
                                       .FirstOrDefault(i => i.StartTag.Name.ToString().Equals("summary", StringComparison.OrdinalIgnoreCase));
            }

            return summary;
        }
    }
}