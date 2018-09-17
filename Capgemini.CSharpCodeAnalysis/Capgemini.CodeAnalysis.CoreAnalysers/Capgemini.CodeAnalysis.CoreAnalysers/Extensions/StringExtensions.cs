﻿using System.Text.RegularExpressions;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Extensions
{
    internal static class StringExtensions
    {
        internal static bool IsGeneratedFileName(this string filePath)
        {
            return
                Regex.IsMatch(filePath,
                    @"(\b(\\bin|\\service|\\obj|\\TemporaryGeneratedFile_|assemblyinfo|assemblyattributes|designer.|\.generated.|\.g.|\.i.)\b)",
                    RegexOptions.IgnoreCase);
        }
    }
}