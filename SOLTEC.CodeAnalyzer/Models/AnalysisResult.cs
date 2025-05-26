
namespace SOLTEC.CodeAnalyzer.Models;

/// <summary>
/// Represents the result of analyzing a single C# file.
/// </summary>
/// <example>
/// <![CDATA[
/// var _result = new AnalysisResult
/// {
///     FilePath = "C:\\Project\\MyClass.cs",
///     Violations = new List<string> { "Missing XML summary", "Namespace is incorrect" }
///     Alerts = new List<string> { "Constructor is empty." }
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
    public List<string> Violations { get; set; } = [];
    /// <summary>
    /// List of descriptive alerts messages found in the file.
    /// </summary>
    public List<string> Alerts { get; set; } = [];
}