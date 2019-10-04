// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1413:Use trailing comma in multi-line initializers", Justification = "Too strict")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:Using directives should be placed correctly", Justification = "Seems to ignore the stylecop.json saying we don't want this.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "We do not currently support localization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:File should have header", Justification = "We don't currently need the file header.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "The test names should speak for themselves.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:FileMustHaveHeader", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "This rule breaks 'Should not be preceeded by a space' so disabling it.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1131:Constant values should appear on the right-hand side of comparisons", Justification = "That depends on your background...")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "For tests, we're happy with the order the methods were created.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "For tests, we're happy with the order the methods were created.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "The test runner will inject, for tests, we do not need to validate.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "CS1591:Missing XML comment for publicly visible type or member ", Justification = "The tests should not need documenting - if they do, they are not clear enough.")]