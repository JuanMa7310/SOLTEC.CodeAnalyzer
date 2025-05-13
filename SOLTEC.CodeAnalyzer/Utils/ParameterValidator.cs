// File: ParameterValidator.cs

namespace SOLTEC.CodeAnalyzer.Utils;

/// <summary>
/// Validates and parses command-line parameters for the analyzer.
/// </summary>
/// <example>
/// <![CDATA[
/// if (!ParameterValidator.Validate(args, out string input, out string output, out string error))
/// {
///     Console.WriteLine(error);
///     return;
/// }
/// ]]>
/// </example>
public static class ParameterValidator
{
    /// <summary>
    /// Validates the command-line arguments.
    /// </summary>
    /// <param name="args">The input arguments array.</param>
    /// <param name="projectPath">Output: path to the project to analyze.</param>
    /// <param name="reportPath">Output: path where the Markdown report should be saved.</param>
    /// <param name="errorMessage">Output: error message if validation fails.</param>
    /// <returns>True if the arguments are valid; otherwise, false.</returns>
    public static bool Validate(string[] args, out string projectPath, out string reportPath, out string errorMessage)
    {
        projectPath = string.Empty;
        reportPath = string.Empty;
        errorMessage = string.Empty;

        if (args.Length != 2)
        {
            errorMessage = "❌ Error: You must provide exactly 2 arguments: <project_path> <report_path>";
            return false;
        }

        projectPath = args[0];
        reportPath = args[1];

        if (!Directory.Exists(projectPath))
        {
            errorMessage = $"❌ Error: The specified project path does not exist: {projectPath}";
            return false;
        }

        return true;
    }
}