using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers
{

    /// <summary>
    /// Analyzes the presence and completeness of XML documentation in C# types,
    /// including classes, records, interfaces, enums, structs, and delegates.
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
        /// Analyzes a .cs file for XML documentation compliance on public/protected types and members.
        /// </summary>
        /// <param name="fileContent">The source code of the file.</param>
        /// <returns>A list of rule violations found in the file.</returns>
        public static List<string> AnalyzeDocumentation(string fileContent)
        {
            var _errors = new List<string>();

            // Common patterns
            var _summaryPattern = @"///\s*<summary>.*?</summary>";
            var _examplePattern = @"///\s*<example>.*?</example>";

            // Unified type detection (class, record, interface, enum, struct, delegate)
            var _typePatterns = new Dictionary<string, string>
        {
            { "class", @"(?:(public|protected)\s+)?(?:abstract\s+|static\s+)?class\s+(\w+)" },
            { "record", @"(?:(public|protected)\s+)?(?:partial\s+)?record\s+(\w+)" },
            { "interface", @"(?:(public|protected)\s+)?interface\s+(\w+)" },
            { "enum", @"(?:(public|protected)\s+)?enum\s+(\w+)" },
            { "struct", @"(?:(public|protected)\s+)?(?:readonly\s+)?struct\s+(\w+)" },
            { "delegate", @"(?:(public|protected)\s+)?delegate\s+[\w<>\[\],\s]+\s+(\w+)\s*\(" }
        };

            foreach (var _type in _typePatterns)
            {
                var _matches = Regex.Matches(fileContent, _type.Value, RegexOptions.Multiline);
                foreach (Match _match in _matches)
                {
                    string _typeName = _match.Groups[2].Success ? _match.Groups[2].Value : _match.Groups[1].Value;
                    string _fullPattern = $@"({_summaryPattern}.*?{_examplePattern})\s*(public|protected).*?\s+{_type.Key}\s+{_typeName}";

                    if (!Regex.IsMatch(fileContent, _fullPattern, RegexOptions.Singleline))
                    {
                        _errors.Add($"{_type.Key.First().ToString().ToUpper() + _type.Key.Substring(1)} '{_typeName}' is missing complete XML documentation (summary + example).");
                    }
                }
            }

            // Métodos
            var _methodMatches = Regex.Matches(fileContent, @"(?:(public|protected)\s+)?(?:static\s+)?[\w<>]+\s+(\w+)\s*\(.*?\)", RegexOptions.Multiline);
            foreach (Match _match in _methodMatches)
            {
                string _methodName = _match.Groups[2].Value;
                string _methodPattern = $@"({_summaryPattern}.*?{_examplePattern})\s*(public|protected).*?\s+{_methodName}\s*\(";

                if (!Regex.IsMatch(fileContent, _methodPattern, RegexOptions.Singleline))
                {
                    _errors.Add($"Method '{_methodName}' is missing complete XML documentation (summary + example).");
                }
            }

            // Propiedades
            var _propertyMatches = Regex.Matches(fileContent, @"(?:(public|protected)\s+)?[\w<>]+\s+(\w+)\s*{", RegexOptions.Multiline);
            foreach (Match _match in _propertyMatches)
            {
                string _propName = _match.Groups[2].Value;
                string _propPattern = $@"({_summaryPattern})\s*(public|protected).*?\s+{_propName}\s*{{";

                if (!Regex.IsMatch(fileContent, _propPattern, RegexOptions.Singleline))
                {
                    _errors.Add($"Property '{_propName}' is missing XML documentation (summary).");
                }
            }

            return _errors;
        }
    }
}