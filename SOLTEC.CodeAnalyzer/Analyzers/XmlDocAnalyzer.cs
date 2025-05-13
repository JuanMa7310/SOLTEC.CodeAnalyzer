// File: XmlDocAnalyzer.cs

namespace SOLTEC.CodeAnalyzer.Analyzers;

using System.Text.RegularExpressions;

/// <summary>
/// Analyzes the presence and completeness of XML documentation in C# code.
/// </summary>
/// <example>
/// <![CDATA[
/// var result = XmlDocAnalyzer.AnalyzeDocumentation(fileContent);
/// foreach (var error in result)
/// {
///     Console.WriteLine(error);
/// }
/// ]]>
/// </example>
public static class XmlDocAnalyzer
{
    /// <summary>
    /// Analyzes a .cs file for XML documentation compliance on public/protected members.
    /// </summary>
    /// <param name="fileContent">The source code of the file.</param>
    /// <returns>A list of rule violations found in the file.</returns>
    public static List<string> AnalyzeDocumentation(string fileContent)
    {
        var _errors = new List<string>();

        // Regex for detecting documented elements
        var _docRegex = new Regex(@"///\s*<summary>.*?</summary>", RegexOptions.Singleline);
        var _exampleRegex = new Regex(@"///\s*<example>.*?</example>", RegexOptions.Singleline);

        // Check for classes
        var _classMatches = Regex.Matches(fileContent, @"(?:(public|protected)\s+)?(?:abstract\s+|static\s+)?class\s+(\w+)", RegexOptions.Multiline);
        foreach (Match _match in _classMatches)
        {
            string _className = _match.Groups[2].Value;
            string _classPattern = $@"(///\s*<summary>.*?</summary>.*?<example>.*?</example>)\s*(public|protected).*class\s+{_className}";

            if (!Regex.IsMatch(fileContent, _classPattern, RegexOptions.Singleline))
            {
                _errors.Add($"Class '{_className}' is missing complete XML documentation (summary + example).");
            }
        }

        // Check for methods
        var _methodMatches = Regex.Matches(fileContent, @"(?:(public|protected)\s+)?(?:static\s+)?[\w<>]+\s+(\w+)\s*\(.*?\)", RegexOptions.Multiline);
        foreach (Match _match in _methodMatches)
        {
            string _methodName = _match.Groups[2].Value;
            string _methodPattern = $@"(///\s*<summary>.*?</summary>.*?<example>.*?</example>)\s*(public|protected).*?\s+{_methodName}\s*\(";

            if (!Regex.IsMatch(fileContent, _methodPattern, RegexOptions.Singleline))
            {
                _errors.Add($"Method '{_methodName}' is missing complete XML documentation (summary + example).");
            }
        }

        // Check for properties (summary only)
        var _propertyMatches = Regex.Matches(fileContent, @"(?:(public|protected)\s+)?[\w<>]+\s+(\w+)\s*{", RegexOptions.Multiline);
        foreach (Match _match in _propertyMatches)
        {
            string _propName = _match.Groups[2].Value;
            string _propPattern = $@"(///\s*<summary>.*?</summary>)\s*(public|protected).*?\s+{_propName}\s*{{";

            if (!Regex.IsMatch(fileContent, _propPattern, RegexOptions.Singleline))
            {
                _errors.Add($"Property '{_propName}' is missing XML documentation (summary).");
            }
        }

        return _errors;
    }
}