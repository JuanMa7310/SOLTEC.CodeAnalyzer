using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Validates that the namespace of a file is correct according to the SOLTEC standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var (_isValid, _error) = NamespaceAnalyzer.AnalyzeNamespace(filePath, projectRoot, projectName);
/// ]]>
/// </example>
public static partial class NamespaceAnalyzer
{
    /// <summary>
    /// Analyzes the namespace declaration in a C# file and verifies it matches the expected structure.
    /// </summary>
    /// <param name="filePath">The full path of the C# file to analyze.</param>
    /// <param name="projectRoot">The root directory of the project containing the .csproj file.</param>
    /// <returns>
    /// A tuple indicating whether the namespace is valid (<c>true</c>) and an optional error message (<c>string</c>).
    /// </returns>
    /// <example>
    /// <![CDATA[
    /// var result = NamespaceAnalyzer.AnalyzeNamespace("Controllers/UserController.cs", "/repos/SOLTEC.Core");
    /// if (!result.isValid)
    ///     Console.WriteLine(result.errorMessage);
    /// ]]>
    /// </example>
    public static (bool isValid, string errorMessage) AnalyzeNamespace(string filePath, string projectRoot)
    {
        (bool, string) _result;

        try
        {
            string _projectName = Path.GetFileNameWithoutExtension(
                Directory.GetFiles(projectRoot, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault()
                ?? throw new FileNotFoundException("No .csproj file found in project root.")
            );
            string _relativePath = Path.GetRelativePath(projectRoot, filePath);
            string _directory = Path.GetDirectoryName(_relativePath) ?? string.Empty;
            string _expectedNamespace = _directory == string.Empty
                ? _projectName
                : _projectName + "." + _directory.Replace(Path.DirectorySeparatorChar, '.');
            string _fileContent = File.ReadAllText(filePath);

            Match _namespaceMatch = NamespacePattern().Match(_fileContent);
            if (!_namespaceMatch.Success)
            {
                _result = (false, "No namespace declaration found.");
            }
            string _actualNamespace = File.ReadAllLines(filePath)
                .FirstOrDefault(line => line.TrimStart().StartsWith("namespace"))?
                .Replace("namespace", string.Empty)
                .Replace(";", string.Empty)
                .Trim() ?? string.Empty;
            if (_actualNamespace != _expectedNamespace)
            {
                _result = (false, $"Namespace mismatch. Expected: '{_expectedNamespace}', Found: '{_actualNamespace}'");
            }
            else
            {
                _result = (true, string.Empty);
            }
        }
        catch (Exception ex)
        {
            _result = (false, $"Error analyzing namespace: {ex.Message}");
        }

        return _result;
    }

    [GeneratedRegex(@"^\s*namespace\s+[A-Za-z0-9_.]+\s*;?", RegexOptions.Multiline)]
    private static partial Regex NamespacePattern();
}