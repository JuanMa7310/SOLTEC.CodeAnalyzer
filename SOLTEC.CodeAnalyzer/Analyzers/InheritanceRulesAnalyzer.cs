using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Validates that public classes are properly restricted for inheritance,
/// requiring them to be `sealed`, `abstract`, or to define at least one `virtual` method.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = InheritanceRulesAnalyzer.AnalyzeInheritanceControl(fileContent);
/// ]]>
/// </example>
public static class InheritanceRulesAnalyzer
{
    /// <summary>
    /// Checks that public classes are not unintentionally inheritable.
    /// </summary>
    /// <param name="fileContent">The full content of the C# file.</param>
    /// <returns>List of violations found regarding inheritance control.</returns>
    public static List<string> AnalyzeInheritanceControl(string fileContent)
    {
        var _violations = new List<string>();

        // Regex to find public classes that are NOT sealed or abstract
        var _publicClassRegex = new Regex(@"\bpublic\s+(?!sealed\b)(?!abstract\b)[^\n]*?\bclass\s+(\w+)", RegexOptions.Multiline);
        var _matches = _publicClassRegex.Matches(fileContent);

        foreach (Match _match in _matches)
        {
            string _className = _match.Groups[1].Value;

            // Check if there is any virtual method defined in the file
            bool _hasVirtualMethod = Regex.IsMatch(
                fileContent,
                @$"\b(public|protected)\s+(virtual)\s+[\w<>\[\]]+\s+\w+\s*\(",
                RegexOptions.Multiline);

            if (!_hasVirtualMethod)
            {
                _violations.Add($"Public class '{_className}' must be declared as sealed, abstract, or contain at least one virtual method.");
            }
        }

        return _violations;
    }
}
