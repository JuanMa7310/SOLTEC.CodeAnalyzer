#!/bin/bash
echo "=== SOLTEC.CodeAnalyzer ==="
read -p "Enter the project path to analyze: " projectPath
read -p "Enter the output report path (.md): " reportPath
read -p "Also print to console? (y/n): " printConsole

extraFlag=""
if [[ "$printConsole" == "y" || "$printConsole" == "Y" ]]; then
    extraFlag="-c"
fi

dotnet run --project SOLTEC.CodeAnalyzer -p "$projectPath" -o "$reportPath" $extraFlag
