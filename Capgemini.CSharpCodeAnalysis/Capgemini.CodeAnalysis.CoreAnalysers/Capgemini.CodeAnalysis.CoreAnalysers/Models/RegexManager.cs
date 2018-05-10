using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Text.RegularExpressions;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Models
{
    /// <summary>
    /// This class provides an abstraction for performing regex operations on string
    /// </summary>
    public class RegexManager
    {
        private readonly DiagnosticsManager _diagnostics=new DiagnosticsManager();

        /// <summary>
        /// Determines in a pattern is found with an input string.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="inputString">The input string.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public bool MatchFound(string pattern, string inputString, RegexOptions options = RegexOptions.None)
        {
            var regex = new Regex(pattern, options);
            var match = regex.Match(inputString);
            var foundMatch = match.Success;
            return foundMatch;
        }

        /// <summary>
        /// This method we will write a diagnostic if name:
        /// contains two consecutive upper case characters or
        /// does not start with an upper case character or
        /// ends with an uppercase character.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public bool DoesNotSatisfyNonePrivateNameRule(SyntaxNodeAnalysisContext context, string name, Location location, DiagnosticDescriptor rule)
        {
            var analysed = false;
            if (MatchFound(@"[A-Z]{2,}", name) || !MatchFound(@"^[A-Z]", name) || MatchFound(@"[A-Z]$", name))
            {
              _diagnostics. CreateNamingConventionDiagnostic(context, location, rule, $"{name} does not satisfy naming convention. \n{name} must start with one upper case character, \nnot end with uppercase character and not contain two consecutive upper case characters.");
                analysed = true;
            }
            return analysed;
        }

        /// <summary>
        /// This method we will write a diagnostic if name:
        /// contains two consecutive upper case characters or
        /// does not start with an underscore and at least the first two characters after the underscore character are lowercase
        /// ends with an uppercase character
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public bool DoesNotSatisfyPrivateNameRule(SyntaxNodeAnalysisContext context, string name, Location location, DiagnosticDescriptor rule)
        {
            var analysed = false;
            if (MatchFound(@"[A-Z]{2,}", name) || !MatchFound(@"^_[a-z]{2,}", name) || MatchFound(@"[A-Z]$", name))
            {
                _diagnostics.CreateNamingConventionDiagnostic(context, location, rule, $"{name} does not satisfy naming convention. \n{name} must start with underscore character followed by at least two lower case characters, \nnot end with uppercase character and not contain two consecutive upper case characters.");
                analysed = true;
            }
            return analysed;
        }

        /// <summary>
        /// This method we will write a diagnostic if name:
        /// contains two consecutive upper case characters or
        /// does not start with at least two lower case characters
        /// ends with an uppercase character
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param> 
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public bool DoesNotSatisfyLocalVariableNameRule(SyntaxNodeAnalysisContext context, string name, Location location, DiagnosticDescriptor rule)
        {
            var analysed = false;
            if (MatchFound(@"[A-Z]{2,}", name) || !MatchFound(@"^[a-z]{2,}", name) || MatchFound(@"[A-Z]$", name))
            {
                _diagnostics.CreateNamingConventionDiagnostic(context, location, rule, $"{name} does not satisfy naming convention. \n{name} must start with at least two lower case character, \nnot end with uppercase character and not contain two consecutive upper case characters.");
                analysed = true;
            }
            return analysed;
        }

        /// <summary>
        /// This method we will write a diagnostic if name:
        /// does not begin with upper case character I, uppercase character and any other lowercase characters
        /// contains two consecutive upper case characters after the first two upper case  characters
        /// ends with an uppercase character
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public bool DoesNotSatisfyInterfaceRule(SyntaxNodeAnalysisContext context, string name, Location location, DiagnosticDescriptor rule)
        {
            var analysed = false;
            if (!MatchFound(@"^I[A-Z][a-z]", name) || MatchFound(@"^I[A-Z](.)*[A-Z]{2,}", name) ||  MatchFound(@"[A-Z]$", name))
            {
                _diagnostics.CreateNamingConventionDiagnostic(context, location, rule, $"{name} does not satisfy naming convention. \n{name} must start with character I and an upper case character, \nnot end with uppercase character and not contain two consecutive upper case characters apart from the first two characters.");
                analysed = true;
            }
            return analysed;
        }

        /// <summary>
        /// Numbers the of lines.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public int NumberOfLines(string inputString, string pattern= "\\r\\n")
        {
            var regex = new Regex(pattern, RegexOptions.None);
            var matches = regex.Matches(inputString);
            return matches.Count;
        }
    }
}
