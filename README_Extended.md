
# SOLTEC.CodeAnalyzer

## 🔍 Description
`SOLTEC.CodeAnalyzer` is a console application that scans and validates C# source code based on SOLTEC's internal coding standards. It is ideal for maintaining consistency, clarity, and professionalism across software projects.

## 📁 Project Structure
```
SOLTEC.CodeAnalyzer/
├── Analyzers/               # Rule checkers (naming, XML docs, inheritance...)
├── Models/                  # Data structures for results and violations
├── Report/                  # Markdown report generator
├── Utils/                   # Parameter parsing, project type detection
├── Program.cs               # Main entry point
├── README.md                # Basic readme (editable)
```

## 🚀 Usage
```bash
dotnet run -- -p "<path_to_project>" -o "<output_report_path>" -c
```

### Parameters
| Flag        | Alias        | Description                                |
|-------------|--------------|--------------------------------------------|
| `-p`        | `--project`  | Path to the project to analyze             |
| `-o`        | `--output`   | Path to save the Markdown report           |
| `-c`        | `--console`  | Optional, prints violations to console     |

## 📦 Output
- A Markdown report listing all rule violations grouped by file.
- Optional real-time feedback in the terminal.

## 🧪 Checks Implemented
- Namespace location and structure
- Naming rules (fields, methods, variables)
- XML documentation with examples
- Type design (sealed/abstract/virtual)
- Inheritance and interface implementation
- Unused parameters, long methods, catch blocks
