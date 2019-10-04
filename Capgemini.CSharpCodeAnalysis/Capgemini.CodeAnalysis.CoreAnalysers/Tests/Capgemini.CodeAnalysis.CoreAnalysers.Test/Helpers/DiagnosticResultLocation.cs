using System;

namespace TestHelper
{
    /// <summary>
    /// Location where the diagnostic appears, as determined by path, line number, and column number.
    /// </summary>
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DiagnosticResultLocation
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public DiagnosticResultLocation(string path, int line, int column)
        {
            if (line < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(line), "line must be >= -1");
            }

            if (column < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "column must be >= -1");
            }

            Path = path;
            Line = line;
            Column = column;
        }

        public string Path { get; }

        public int Line { get; }

        public int Column { get; }
    }
}