// File: AnalysisResult.cs

namespace SOLTEC.CodeAnalyzer.Models;

using System.Collections.Generic;

/// <summary>
/// Represents the result of analyzing a single C# file.
/// </summary>
/// <example>
/// <![CDATA[
/// var result = new AnalysisResult
/// {
///     FilePath = "C:\\Project\\MyClass.cs",
///     Violations = new List<string> { "Missing XML summary", "Namespace is incorrect" }
/// };
/// ]]>
/// </example>
public class AnalysisResult
{
    /// <summary>
    /// Full path to the C# file analyzed.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// List of descriptive violation messages found in the file.
    /// </summary>
    public List<string> Violations { get; set; } = new();
}