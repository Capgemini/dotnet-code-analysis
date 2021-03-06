﻿using System;
using Microsoft.CodeAnalysis;

namespace TestHelper
{
    /// <summary>
    /// Struct that stores information about a Diagnostic appearing in a source.
    /// </summary>
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DiagnosticResult
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        private DiagnosticResultLocation[] locations;

#pragma warning disable CA1819 // Properties should not return arrays
        public DiagnosticResultLocation[] Locations
#pragma warning restore CA1819 // Properties should not return arrays
        {
            get
            {
                if (locations == null)
                {
                    locations = Array.Empty<DiagnosticResultLocation>();
                }

                return locations;
            }

            set => locations = value;
        }

        public DiagnosticSeverity Severity { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public string Path => Locations.Length > 0 ? Locations[0].Path : string.Empty;

        public int Line => Locations.Length > 0 ? Locations[0].Line : -1;

        public int Column => Locations.Length > 0 ? Locations[0].Column : -1;
    }
}