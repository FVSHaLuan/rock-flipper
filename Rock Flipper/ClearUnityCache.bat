@echo off
title Unity Project Cache Cleaner

echo ==========================================
echo     Unity Project Cache Cleaner
echo ==========================================
echo.
echo This will delete Unity-generated cache folders:
echo.
echo   - Library
echo   - Logs
echo   - Temp
echo   - obj / Obj
echo   - .vs
echo   - Build / Builds
echo   - Generated solution files
echo.
echo Your Assets and ProjectSettings will NOT be deleted.
echo.

choice /c YN /m "Continue"

if errorlevel 2 (
    echo.
    echo Operation cancelled.
    pause
    exit /b
)

echo.
echo Cleaning Unity cache folders...
echo.

REM Move to the folder where this BAT file is located
cd /d "%~dp0"

REM Delete common Unity-generated folders
for %%F in (
    Library
    Logs
    Temp
    Obj
    obj
    Build
    Builds
    MemoryCaptures
    UserSettings
) do (
    if exist "%%F" (
        echo Deleting %%F ...
        rmdir /s /q "%%F"
    )
)

REM Delete IDE-generated folders
for %%F in (
    .vs
) do (
    if exist "%%F" (
        echo Deleting %%F ...
        rmdir /s /q "%%F"
    )
)

REM Delete generated solution/project files
del /q *.csproj >nul 2>&1
del /q *.sln >nul 2>&1
del /q *.user >nul 2>&1
del /q *.pidb >nul 2>&1
del /q *.booproj >nul 2>&1
del /q *.svd >nul 2>&1
del /q *.pdb >nul 2>&1
del /q *.mdb >nul 2>&1
del /q *.opendb >nul 2>&1
del /q *.VC.db >nul 2>&1

echo.
echo Done cleaning Unity cache files.
echo.
pause