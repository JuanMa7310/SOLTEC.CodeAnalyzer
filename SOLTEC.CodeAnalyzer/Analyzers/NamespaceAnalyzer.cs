namespace SOLTEC.CodeAnalyzer.Analyzers;

using System.Text.RegularExpressions;

/// <summary>
/// Validates that the namespace of a file is correct according to the SOLTEC standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var (isValid, error) = NamespaceAnalyzer.AnalyzeNamespace(filePath, fileContent, projectRoot);
/// ]]>
/// </example>
public static partial class NamespaceAnalyzer
{
    /// <summary>
    /// Analyzes the namespace declaration in a C# file.
    /// </summary>
    /// <param name="filePath">The full path of the file.</param>
    /// <param name="fileContent">The content of the file.</param>
    /// <param name="projectRoot">The root path of the project.</param>
    /// <returns>A tuple indicating whether the namespace is valid and an optional error message.</returns>
    public static (bool isValid, string errorMessage) AnalyzeNamespace(string filePath, string fileContent, string projectRoot)
    {
        var _relativePath = filePath.Replace(projectRoot, "").Replace("\\", "/").Trim('/');
        var _expectedNamespace = "SOLTEC." + string.Join('.', _relativePath.Split('/')
            .Where(segment => !segment.EndsWith(".cs"))
            .Select(segment => segment.Replace(".cs", "")));

        var _lines = fileContent.Split('\n');
        var _namespaceLine = _lines.FirstOrDefault(l => l.TrimStart().StartsWith("namespace "));

        if (string.IsNullOrWhiteSpace(_namespaceLine))
            return (false, "No namespace declared.");

        var _match = NamespacePattern().Match(_namespaceLine.Trim());
        if (!_match.Success)
            return (false, "Namespace declaration is invalid.");

        var _declaredNamespace = _match.Value.Replace("namespace", "").Trim().TrimEnd(';');
        if (_declaredNamespace != _expectedNamespace)
        {
            return (false, $"Namespace mismatch. Expected '{_expectedNamespace}' but found '{_declaredNamespace}'.");
        }

        return (true, "");
    }

    [GeneratedRegex(@"^\s*namespace\s+[A-Za-z0-9_.]+\s*;?", RegexOptions.Multiline)]
    private static partial Regex NamespacePattern();
}