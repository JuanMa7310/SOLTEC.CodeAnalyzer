namespace SOLTEC.CodeAnalyzer.Models;

/// <summary>
/// Represents a detailed rule violation found during analysis.
/// </summary>
/// <example>
/// <![CDATA[
/// var detail = new ViolationDetail
/// {
///     Rule = "Missing XML summary",
///     LineNumber = 42
/// };
/// ]]>
/// </example>
public class ViolationDetail
{
    /// <summary>
    /// Description of the violated rule.
    /// </summary>
    public string Rule { get; set; } = string.Empty;

    /// <summary>
    /// Optional: Line number where the violation was detected.
    /// </summary>
    public int LineNumber { get; set; } = -1;
}