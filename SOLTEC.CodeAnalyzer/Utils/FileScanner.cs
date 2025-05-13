namespace SOLTEC.CodeAnalyzer.Utils;

/// <summary>
/// Provides utility methods to retrieve C# source files from a directory.
/// </summary>
/// <example>
/// <![CDATA[
/// var files = FileScanner.GetCsFiles("C:\\MyProject");
/// foreach (var f in files)
/// {
///     Console.WriteLine(f);
/// }
/// ]]>
/// </example>
public static class FileScanner
{
    /// <summary>
    /// Recursively retrieves all .cs files from the given base directory.
    /// </summary>
    /// <param name="baseDirectory">The root directory to scan.</param>
    /// <returns>List of full paths to .cs files.</returns>
    public static List<string> GetCsFiles(string baseDirectory)
    {
        if (!Directory.Exists(baseDirectory))
            return [];

        return [.. Directory.GetFiles(baseDirectory, "*.cs", SearchOption.AllDirectories)];
    }
}