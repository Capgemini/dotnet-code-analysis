using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Models
{
    /// <summary>
    /// This class is responsible for manipulating code comments
    /// </summary>
    public class CommentsManager
    {
        public string Test;
        public bool HasValidComments(SyntaxNode node)
        {
            var result = HasValidSummaryComments(node);

            if (!result)
            {
                //now let's test for other types of comments
                var comments = ExtractLeadingComments(node);

                result = ProcessComments(comments);
            }

            return result;
        }

        private static bool ProcessComments(List<SyntaxTrivia> comments)
        {
            var result = false;
            if (comments.Count == 1)
            {
                var comment = comments[0];
                var content = comment.ToString().Replace(" ", "").Trim();
                if (!( content == "//"  || content == "/**/" ))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool FieldHasValidComments(SyntaxNode node)
        {
            var result = HasValidSummaryComments(node);

            if (!result)
            {
                //now let's test for other types of comments
                var comments = ExtractLeadingComments(node);
                comments.AddRange(ExtractTrailingComments(node));

                result = ProcessComments(comments);
            }

            return result;
        }

        /// <summary>
        /// Extracts the documentation comment.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public DocumentationCommentTriviaSyntax ExtractDocumentationComment(SyntaxNode node)
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

        public List<SyntaxTrivia> ExtractComment(SyntaxNode node)
        {
            var result = ExtractLeadingComments(node);
            return result;
        }

        /// <summary>
        /// Determines whether [has valid summary comments] [the specified node].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if [has valid summary comments] [the specified node]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValidSummaryComments(SyntaxNode node)
        {
            var result = false;

            var summary = GetSummaryComment(node);
            if (summary != null)
            {
                var s = summary.Content.ToFullString()?.Replace("///", "");
                result = !string.IsNullOrWhiteSpace(s);
            }
            return result;
        }

        /// <summary>
        /// Gets the summary comment.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public XmlElementSyntax GetSummaryComment(SyntaxNode node)
        {
            return ExtractSummaryComment(node);
        }

        public List<SyntaxTrivia> RetrieveMultiLineComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments.AddRange( node.GetLeadingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) )
                       .ToList());
                comments.AddRange(node.GetTrailingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia))
                       .ToList());
            }

            return comments;
        }

        private XmlElementSyntax ExtractSummaryComment(SyntaxNode node)
        {
            XmlElementSyntax summary = null;
            if (node != null && node.HasStructuredTrivia)
            {
                var docummentation = ExtractDocumentationComment(node);
                summary = docummentation?.ChildNodes()
                                       .OfType<XmlElementSyntax>()
                                       .FirstOrDefault(i => i.StartTag.Name.ToString().Equals("summary"));

            }
            return summary;
        }
        
        private List<SyntaxTrivia> ExtractLeadingComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments = node.GetLeadingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                                 a.IsKind(SyntaxKind.SingleLineCommentTrivia)
                       )
                       .ToList();
            }
            return comments;
        }

        private List<SyntaxTrivia> ExtractTrailingComments(SyntaxNode node)
        {
            var comments = new List<SyntaxTrivia>();
            if (node != null)
            {
                comments = node.GetTrailingTrivia()
                       .Where(a => a.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                                 a.IsKind(SyntaxKind.SingleLineCommentTrivia)
                       )
                       .ToList();
            }
            return comments;
        }
    }
}
