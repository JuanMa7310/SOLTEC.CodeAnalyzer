using SOLTEC.CodeAnalyzer.Utils;
using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;


/// <summary>
/// Analyzes XML documentation compliance in C# files.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = XmlDocAnalyzer.AnalyzeDocumentation(code, ProjectTypeDetector.ProjectType.WebApi);
/// ]]>
/// </example>
public static class XmlDocAnalyzer
{
    /// <summary>
    /// Analyzes a C# file for documentation compliance based on the project type.
    /// </summary>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="projectType">Type of project for visibility rules.</param>
    /// <returns>List of violations.</returns>
    public static List<string> AnalyzeDocumentation(string fileContent, ProjectTypeDetector.ProjectType projectType)
    {
        var _errors = new List<string>();

        string _summaryPattern = @"///\s*<summary>.*?</summary>";
        string _examplePattern = @"///\s*<example>.*?</example>";

        var _typePatterns = new Dictionary<string, string>
    {
        { "class", @"(?:(public|protected|protected\s+internal)\s+)?(?:abstract\s+|static\s+)?class\s+(\w+)" },
        { "record", @"(?:(public|protected|protected\s+internal)\s+)?(?:partial\s+)?record\s+(\w+)" },
        { "interface", @"(?:(public|protected|protected\s+internal)\s+)?interface\s+(\w+)" },
        { "enum", @"(?:(public|protected|protected\s+internal)\s+)?enum\s+(\w+)" },
        { "struct", @"(?:(public|protected|protected\s+internal)\s+)?(?:readonly\s+)?struct\s+(\w+)" },
        { "delegate", @"(?:(public|protected|protected\s+internal)\s+)?delegate\s+[\w<>\[\],\s]+\s+(\w+)\s*\(" }
    };

        bool _includeInternal = projectType is ProjectTypeDetector.ProjectType.ConsoleApp;

        foreach (var _type in _typePatterns)
        {
            var _matches = Regex.Matches(fileContent, _type.Value, RegexOptions.Multiline);
            foreach (Match _match in _matches)
            {
                string _access = _match.Groups[1].Value?.Trim() ?? "";
                string _typeName = _match.Groups[2].Success ? _match.Groups[2].Value : "";

                if (!_includeInternal &&
                    _access != "public" && _access != "protected" && _access != "protected internal")
                {
                    continue;
                }

                string _fullPattern = $@"({_summaryPattern}.*?{_examplePattern})\s*(public|protected|protected\s+internal).*?\s+{_type.Key}\s+{_typeName}";

                if (!Regex.IsMatch(fileContent, _fullPattern, RegexOptions.Singleline))
                {
                    _errors.Add($"{_type.Key.First().ToString().ToUpper() + _type.Key[1..]} '{_typeName}' is missing complete XML documentation (summary + example).");
                }
            }
        }

        // Métodos
        var _methodMatches = Regex.Matches(fileContent, @"(?:(public|protected|protected\s+internal)\s+)?(?:static\s+)?[\w<>]+\s+(\w+)\s*\(.*?\)", RegexOptions.Multiline);
        foreach (Match _match in _methodMatches)
        {
            string _access = _match.Groups[1].Value?.Trim() ?? "";
            string _methodName = _match.Groups[2].Value;

            if (!_includeInternal &&
                _access != "public" && _access != "protected" && _access != "protected internal")
            {
                continue;
            }

            string _methodPattern = $@"({_summaryPattern}.*?{_examplePattern})\s*(public|protected|protected\s+internal).*?\s+{_methodName}\s*\(";

            if (!Regex.IsMatch(fileContent, _methodPattern, RegexOptions.Singleline))
            {
                _errors.Add($"Method '{_methodName}' is missing complete XML documentation (summary + example).");
            }
        }

        // Propiedades
        var _propertyMatches = Regex.Matches(fileContent, @"(?:(public|protected|protected\s+internal)\s+)?[\w<>]+\s+(\w+)\s*{", RegexOptions.Multiline);
        foreach (Match _match in _propertyMatches)
        {
            string _access = _match.Groups[1].Value?.Trim() ?? "";
            string _propName = _match.Groups[2].Value;

            if (!_includeInternal &&
                _access != "public" && _access != "protected" && _access != "protected internal")
            {
                continue;
            }

            string _propPattern = $@"({_summaryPattern})\s*(public|protected|protected\s+internal).*?\s+{_propName}\s*{{";

            if (!Regex.IsMatch(fileContent, _propPattern, RegexOptions.Singleline))
            {
                _errors.Add($"Property '{_propName}' is missing XML documentation (summary).");
            }
        }

        return _errors;
    }
}