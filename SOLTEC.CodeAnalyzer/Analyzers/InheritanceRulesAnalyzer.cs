namespace SOLTEC.CodeAnalyzer.Analyzers;

using System.Text.RegularExpressions;

/// <summary>
/// Validates that public classes are properly restricted for inheritance,
/// requiring them to be `sealed`, `abstract`, or to define at least one `virtual` method.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = InheritanceRulesAnalyzer.AnalyzeInheritanceControl(fileContent);
/// ]]>
/// </example>
public static partial class InheritanceRulesAnalyzer
{
    /// <summary>
    /// Checks that public classes are not unintentionally inheritable.
    /// </summary>
    /// <param name="fileContent">The full content of the C# file.</param>
    /// <returns>List of violations found regarding inheritance control.</returns>
    public static List<string> AnalyzeInheritanceControl(string fileContent)
    {
        var _violations = new List<string>();
        var _matches = PublicClassRegex().Matches(fileContent);

        foreach (Match _match in _matches)
        {
            string _className = _match.Groups[1].Value;

            bool _hasVirtual = VirtualMethodRegex().IsMatch(fileContent);
            if (!_hasVirtual)
            {
                _violations.Add($"Public class '{_className}' must be sealed, abstract, or contain at least one virtual method.");
            }
        }

        return _violations;
    }

    [GeneratedRegex(@"\bpublic\s+(?!sealed\b)(?!abstract\b)[^\n]*?\bclass\s+(\w+)", RegexOptions.Multiline)]
    private static partial Regex PublicClassRegex();

    [GeneratedRegex(@"\b(public|protected)\s+virtual\s+[\w<>\[\]]+\s+\w+\s*\(", RegexOptions.Multiline)]
    private static partial Regex VirtualMethodRegex();
}
