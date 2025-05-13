namespace SOLTEC.CodeAnalyzer.Analyzers;

using System.Text.RegularExpressions;

/// <summary>
/// Performs deep analysis on type and method implementations for code quality rules.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = ImplementationAnalyzer.AnalyzeImplementation(fileContent);
/// ]]>
/// </example>
public static partial class ImplementationAnalyzer
{
    public static List<string> AnalyzeImplementation(string fileContent)
    {
        var _violations = new List<string>();

        _violations.AddRange(DetectEmptyPublicTypes(fileContent));
        _violations.AddRange(DetectUselessPublicMethods(fileContent));
        _violations.AddRange(DetectLongMethods(fileContent));
        _violations.AddRange(DetectUnusedParameters(fileContent));
        _violations.AddRange(DetectEmptyCatchBlocks(fileContent));
        _violations.AddRange(DetectUnusedFields(fileContent));
        _violations.AddRange(DetectEmptyPublicConstructors(fileContent));
        _violations.AddRange(DetectMissingReturnStatements(fileContent));

        return _violations;
    }

    private static List<string> DetectEmptyPublicTypes(string content)
    {
        var _violations = new List<string>();
        var _matches = TypePattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _name = _match.Groups[3].Value;
            string _bodyPattern = $@"{_match.Value}[^\n]*{{(.*?)}}";
            var _bodyMatch = Regex.Match(content, _bodyPattern, RegexOptions.Singleline);

            if (_bodyMatch.Success)
            {
                string _body = _bodyMatch.Groups[1].Value;
                if (!ExposedMemberPattern().IsMatch(_body))
                {
                    _violations.Add($"{_match.Groups[2].Value} '{_name}' is public/protected but exposes no members.");
                }
            }
        }
        return _violations;
    }

    private static List<string> DetectUselessPublicMethods(string content)
    {
        var _violations = new List<string>();
        var _matches = PublicMethodPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _methodName = _match.Groups[2].Value;
            string _body = _match.Groups[3].Value.Trim();

            if (string.IsNullOrWhiteSpace(_body) ||
                CommentOnlyPattern().IsMatch(_body) ||
                NotImplementedPattern().IsMatch(_body) ||
                UselessReturnPattern().IsMatch(_body))
            {
                _violations.Add($"Public method '{_methodName}' has no meaningful implementation.");
            }
        }

        return _violations;
    }

    private static List<string> DetectLongMethods(string content)
    {
        var _violations = new List<string>();
        var _matches = AnyMethodPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            int _startIndex = _match.Index;
            int _openBraces = 0, _endIndex = -1;

            for (int _i = _startIndex; _i < content.Length; _i++)
            {
                if (content[_i] == '{') _openBraces++;
                else if (content[_i] == '}') _openBraces--;

                if (_openBraces == 0)
                {
                    _endIndex = _i;
                    break;
                }
            }

            if (_endIndex > _startIndex)
            {
                string _methodBlock = content[_startIndex.._endIndex];
                int _lineCount = _methodBlock.Split('\n').Length;

                if (_lineCount > 50)
                {
                    _violations.Add($"Method '{_match.Groups[2].Value}' is too long ({_lineCount} lines). Consider splitting or refactoring.");
                }
            }
        }

        return _violations;
    }

    private static List<string> DetectUnusedParameters(string content)
    {
        var _violations = new List<string>();
        var _matches = MethodWithParamsPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _methodName = _match.Groups[2].Value;
            string[] _params = _match.Groups[3].Value.Split(',');

            string _body = content[_match.Index..];
            int _braceCount = 0, _bodyEnd = -1;

            for (int _i = _match.Index; _i < content.Length; _i++)
            {
                if (content[_i] == '{') _braceCount++;
                else if (content[_i] == '}') _braceCount--;

                if (_braceCount == 0)
                {
                    _bodyEnd = _i;
                    break;
                }
            }

            if (_bodyEnd > _match.Index)
            {
                string _methodBody = content[_match.Index.._bodyEnd];

                foreach (var _param in _params)
                {
                    var _name = _param.Trim().Split(' ').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(_name) &&
                        !Regex.IsMatch(_methodBody, $@"\b{Regex.Escape(_name)}\b"))
                    {
                        _violations.Add($"Parameter '{_name}' in method '{_methodName}' is never used.");
                    }
                }
            }
        }

        return _violations;
    }

    private static List<string> DetectEmptyCatchBlocks(string content)
    {
        var _violations = new List<string>();
        if (CatchEmptyPattern().IsMatch(content))
        {
            _violations.Add("Empty or silent catch block detected. Consider logging or rethrowing.");
        }

        return _violations;
    }

    private static List<string> DetectUnusedFields(string content)
    {
        var _violations = new List<string>();
        var _matches = GlobalFieldPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _fieldName = _match.Groups[2].Value;
            var _references = Regex.Matches(content, $@"\b{Regex.Escape(_fieldName)}\b");

            if (_references.Count <= 1)
            {
                _violations.Add($"Global field '{_fieldName}' is declared but never used.");
            }
        }

        return _violations;
    }

    private static List<string> DetectEmptyPublicConstructors(string content)
    {
        var _violations = new List<string>();
        var _matches = PublicCtorPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _className = _match.Groups[1].Value;
            string _body = _match.Groups[2].Value;

            if (string.IsNullOrWhiteSpace(_body) || CommentOnlyPattern().IsMatch(_body))
            {
                _violations.Add($"Public constructor for '{_className}' is empty.");
            }
        }

        return _violations;
    }

    private static List<string> DetectMissingReturnStatements(string content)
    {
        var _violations = new List<string>();
        var _matches = ReturnableMethodPattern().Matches(content);

        foreach (Match _match in _matches)
        {
            string _returnType = _match.Groups[2].Value;
            string _methodName = _match.Groups[3].Value;

            if (_returnType == "void" || _returnType == "Task" || _returnType.StartsWith("Task<"))
                continue;

            int _startIndex = _match.Index;
            int _braceCount = 0, _endIndex = -1;

            for (int _i = _startIndex; _i < content.Length; _i++)
            {
                if (content[_i] == '{') _braceCount++;
                else if (content[_i] == '}') _braceCount--;

                if (_braceCount == 0)
                {
                    _endIndex = _i;
                    break;
                }
            }

            if (_endIndex > _startIndex)
            {
                string _methodBody = content[_startIndex.._endIndex];

                if (!ReturnKeywordPattern().IsMatch(_methodBody))
                {
                    _violations.Add($"Method '{_methodName}' declares return type '{_returnType}' but may exit without returning.");
                }
            }
        }

        return _violations;
    }

    // -------------------------
    // REGEX HELPERS OPTIMIZED
    // -------------------------

    [GeneratedRegex(@"(public|protected)\s+(class|interface|struct|record|delegate)\s+(\w+)", RegexOptions.Multiline)]
    private static partial Regex TypePattern();

    [GeneratedRegex(@"(public|protected)\s+(event|void|[\w<>\[\]]+\s+\w+)\s*[\(\{]", RegexOptions.Multiline)]
    private static partial Regex ExposedMemberPattern();

    [GeneratedRegex(@"(public)\s+[\w<>\[\]]+\s+(\w+)\s*\([^\)]*\)\s*{([^}]*)}", RegexOptions.Multiline)]
    private static partial Regex PublicMethodPattern();

    [GeneratedRegex(@"^\s*(//.*|\s)*$", RegexOptions.Multiline)]
    private static partial Regex CommentOnlyPattern();

    [GeneratedRegex(@"throw\s+new\s+NotImplementedException\s*\(\s*\);", RegexOptions.Multiline)]
    private static partial Regex NotImplementedPattern();

    [GeneratedRegex(@"return\s+(null|default|0|false)\s*;", RegexOptions.Multiline)]
    private static partial Regex UselessReturnPattern();

    [GeneratedRegex(@"(public|protected|private|internal)\s+[\w<>\[\]]+\s+(\w+)\s*\(.*?\)\s*{", RegexOptions.Multiline)]
    private static partial Regex AnyMethodPattern();

    [GeneratedRegex(@"\b(public|protected|private|internal)\s+[\w<>\[\]]+\s+(\w+)\s*\(([^)]*)\)\s*{", RegexOptions.Multiline)]
    private static partial Regex MethodWithParamsPattern();

    [GeneratedRegex(@"catch\s*\([^\)]*\)\s*{\s*}", RegexOptions.Multiline)]
    private static partial Regex CatchEmptyPattern();

    [GeneratedRegex(@"(private|protected|public)\s+[\w<>\[\]]+\s+(g\w+)\s*(=|;)", RegexOptions.Multiline)]
    private static partial Regex GlobalFieldPattern();

    [GeneratedRegex(@"public\s+(\w+)\s*\([^\)]*\)\s*{([^}]*)}", RegexOptions.Multiline)]
    private static partial Regex PublicCtorPattern();

    [GeneratedRegex(@"(public|protected|private|internal)\s+(\w[\w<>\[\]]+)\s+(\w+)\s*\(.*?\)\s*{", RegexOptions.Multiline)]
    private static partial Regex ReturnableMethodPattern();

    [GeneratedRegex(@"\breturn\b", RegexOptions.Multiline)]
    private static partial Regex ReturnKeywordPattern();
}