# SOLTEC.CodeAnalyzer - Overview

**SOLTEC.CodeAnalyzer** is a professional-grade console utility developed in C# (.NET 8, C# 12) that performs static code analysis on C# source files. It validates adherence to SOLTEC's strict internal programming standards.

## 🔍 What It Does

- Analyzes all `.cs` files in a given project directory.
- Verifies that each type declaration (class, interface, record, enum, struct, delegate) complies with:
  - Namespace rules (starts with `SOLTEC.` and matches folder hierarchy).
  - XML documentation (`<summary>` and `<example>`) for public/protected types and members.
  - Naming conventions for variables, constants, and parameters.
- Supports output to Markdown file and optionally to console.
- Parameters accept professional CLI flags:
  - `-p`: Path to the project folder.
  - `-o`: Output Markdown report file path.
  - `-c`: (Optional) Also print violations to console.

## 🧪 Supported Type Elements

- Classes
- Records
- Interfaces
- Enums
- Structs
- Delegates

## 📤 Output

- Markdown report with:
  - Summary of files analyzed
  - Violations listed per file and per element
- Optional inline console output with the `-c` flag
