@echo off
echo === SOLTEC.CodeAnalyzer ===
set /p projectPath="Enter the project path to analyze: "
set /p reportPath="Enter the output report path (.md): "
set /p printConsole="Also print to console? (y/n): "

set extraFlag=
if /i "%printConsole%"=="y" (
    set extraFlag=-c
)

dotnet run --project SOLTEC.CodeAnalyzer -p "%projectPath%" -o "%reportPath%" %extraFlag%
pause
