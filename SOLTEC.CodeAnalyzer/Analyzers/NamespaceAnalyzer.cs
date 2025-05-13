using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Analyzes whether the namespace of a class follows SOLTEC standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var result = NamespaceAnalyzer.AnalyzeNamespace("C:\\Projects\\SOLTEC\\Core\\Utils\\MyClass.cs");
/// if (!result.IsValid)
/// {
///     Console.WriteLine(result.ErrorMessage);
/// }
/// ]]>
/// </example>
public static class NamespaceAnalyzer
{
    /// <summary>
    /// Validates that the namespace is declared at file level and follows the SOLTEC naming convention.
    /// </summary>
    /// <param name="filePath">The full path to the .cs file.</param>
    /// <param name="fileContent">The content of the file.</param>
    /// <param name="baseDirectory">The root path of the analyzed project (used to validate namespace nesting).</param>
    /// <returns>A tuple indicating whether the namespace is valid and an optional error message.</returns>
    public static (bool IsValid, string ErrorMessage) AnalyzeNamespace(string filePath, string fileContent, string baseDirectory)
    {
        string _relativePath = Path.GetRelativePath(baseDirectory, filePath);
        string _expectedNamespace = "SOLTEC." + Path.GetDirectoryName(_relativePath)
            ?.Replace(Path.DirectorySeparatorChar, '.')
            .TrimEnd('.');

        var _namespaceRegex = new Regex(@"^\s*namespace\s+([A-Za-z0-9_.]+)\s*;", RegexOptions.Multiline);
        var _match = _namespaceRegex.Match(fileContent);

        if (!_match.Success)
        {
            return (false, "Namespace declaration not found or incorrectly placed.");
        }

        string _declaredNamespace = _match.Groups[1].Value.Trim();

        if (!_declaredNamespace.StartsWith("SOLTEC"))
        {
            return (false, $"Namespace must start with 'SOLTEC': found '{_declaredNamespace}'.");
        }

        if (_declaredNamespace != _expectedNamespace)
        {
            return (false, $"Expected namespace '{_expectedNamespace}', but found '{_declaredNamespace}'.");
        }

        return (true, string.Empty);
    }
}