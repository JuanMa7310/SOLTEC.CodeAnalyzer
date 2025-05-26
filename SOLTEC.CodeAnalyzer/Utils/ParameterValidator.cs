
namespace SOLTEC.CodeAnalyzer.Utils;

/// <summary>
/// Parses and validates named command-line parameters for the analyzer.
/// </summary>
/// <example>
/// <![CDATA[
/// if (!ParameterValidator.Validate(args, out var path, out var output, out var toConsole, out var error))
/// {
///     Console.WriteLine(error);
///     return;
/// }
/// ]]>
/// </example>
public static class ParameterValidator
{
    /// <summary>
    /// Validates and extracts named parameters.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <param name="projectPath">Extracted project path.</param>
    /// <param name="outputPath">Extracted output path.</param>
    /// <param name="printToConsole">Flag to print results to console.</param>
    /// <param name="errorMessage">Error message if invalid.</param>
    /// <returns>True if parameters are valid, false otherwise.</returns>
    public static bool Validate(string[] args, out string projectPath, out string outputPath, out bool printToConsole, out string errorMessage)
    {
        projectPath = string.Empty;
        outputPath = string.Empty;
        printToConsole = false;
        errorMessage = string.Empty;

        for (int _i = 0; _i < args.Length; _i++)
        {
            switch (args[_i])
            {
                case "-p":
                case "--project":
                    if (_i + 1 < args.Length) projectPath = args[++_i];
                    break;
                case "-o":
                case "--output":
                    if (_i + 1 < args.Length) outputPath = args[++_i];
                    break;
                case "-c":
                case "--console":
                    printToConsole = true;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(projectPath) || string.IsNullOrWhiteSpace(outputPath))
        {
            errorMessage = """
            ❌ Error: Missing required parameters.

            Usage:
              -p <project_path>         Path to the C# project directory
              --project <project_path>  Path to the C# project directory
              -o <output_path>          Path to the Markdown report file
              --output <output_path>    Path to the Markdown report file
              -c                        (Optional) Also print results to console
              --console                 (Optional) Also print results to console

            Example:
              dotnet run -- -p "C:\\MyProject" -o "C:\\Reports\\output.md" -c
              dotnet run -- --project "C:\\MyProject" -output "C:\\Reports\\output.md" -console
            """;
            return false;
        }

        if (!Directory.Exists(projectPath))
        {
            errorMessage = $"❌ Error: Project path does not exist: {projectPath}";
            return false;
        }
        if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
        {
            errorMessage = $"❌ Error: Project path does not exist: {outputPath}";
            return false;
        }

        return true;
    }
}