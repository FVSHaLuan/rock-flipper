@echo off
setlocal
:: Usage: Archive.bat [folder]
:: Packs, cleans and archives the Windows, Linux and Mac builds in [folder] (default: this script's folder).
:: Each variant folder (Output\Demo, Output\Release, ...) has its own small Archive.bat that calls this one.
:: The game name and version come from the "title" and "version" lines of the builds' buildCfg.ini.

set archiveRoot=Archives
:: Appended to the version folder name when that folder already exists. # is replaced by the number.
set "dupFormat=_#"

if "%~1"=="" (set "target=%~dp0.") else (set "target=%~1")
pushd "%target%" || (
    echo Folder not found: %target%
    pause
    exit /b 1
)
echo Folder: %CD%

::------------ Find platforms that have files
set "toPack="
set "missing="
for %%P in (Linux Mac Windows) do call :CHECK %%P
if not defined toPack (
    echo No builds found in any platform folder.
    goto END
)

::------------ Read the build config (Unity writes the Mac one inside Mac.app)
set "cfgFile="
for %%F in (Windows\buildCfg.ini Linux\buildCfg.ini Mac\Mac.app\buildCfg.ini Mac\buildCfg.ini) do if not defined cfgFile if exist ".\%%F" set "cfgFile=.\%%F"
if not defined cfgFile (
    echo Could not find buildCfg.ini in any platform folder.
    goto END
)

set "gameName="
set "ver="
call :READCFG title gameName
call :READCFG version ver
if not defined gameName (
    echo Could not read the title from %cfgFile%
    goto END
)
if not defined ver (
    echo Could not read the version number from %cfgFile%
    goto END
)

echo game name: %gameName%
echo version number: %ver%
echo platforms to pack:%toPack%
if not defined missing goto CONFIRM
echo missing or empty:%missing%
set "SKIPMISSING="
SET /P SKIPMISSING=Skip the missing platforms and continue, or abort (S/[A])?
IF /I "%SKIPMISSING%" NEQ "S" GOTO END

:CONFIRM
echo This will pack the platforms above, delete the current builds, and move the zips to %archiveRoot%.
set "AREYOUSURE="
SET /P AREYOUSURE=Are you sure you want to continue with above version number (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

::------------ Pack
set "packFailed="
for %%P in (%toPack%) do if not defined packFailed call :PACK %%P
if defined packFailed (
    echo Packing failed, nothing was deleted or archived.
    goto END
)

::------------ Delete
echo Deleting...

del ".\Linux\*" /s /q 2>nul
rmdir ".\Linux" /s /q 2>nul
mkdir ".\Linux"

del ".\Mac\*" /s /q 2>nul
rmdir ".\Mac" /s /q 2>nul
mkdir ".\Mac"
mkdir ".\Mac\Mac.app"

del ".\Windows\*" /s /q 2>nul
rmdir ".\Windows" /s /q 2>nul
mkdir ".\Windows"

::------------ Archive
if not exist ".\%archiveRoot%\" mkdir ".\%archiveRoot%"

set "verDir=%ver%"
set n=0
:FINDNAME
if not exist ".\%archiveRoot%\%verDir%" goto MAKEDIR
set /a n+=1
call set "verDir=%ver%%%dupFormat:#=%n%%%"
goto FINDNAME
:MAKEDIR
mkdir ".\%archiveRoot%\%verDir%"

echo Moving zips to %archiveRoot%\%verDir%...
move /y ".\*.zip" ".\%archiveRoot%\%verDir%\"

echo Done!

:END
popd
pause
exit /b

::------------ Subroutines

:: Adds the platform to toPack if its folder contains at least one file, otherwise to missing.
:CHECK
dir /s /b /a-d ".\%1" 2>nul | findstr "^" >nul
if errorlevel 1 (set "missing=%missing% %1") else (set "toPack=%toPack% %1")
exit /b

:: Sets variable %2 to the value of the "%1 = value" line in cfgFile, without leading spaces.
:READCFG
for /f "tokens=1* delims==" %%A in ('findstr /i /r /c:"^%1 *=" "%cfgFile%"') do for /f "tokens=*" %%V in ("%%B") do set "%2=%%V"
exit /b

:: The trailing-backslash masks exclude whole folders, the others exclude matching files.
:PACK
echo packing %1...
"C:\Program Files\WinRAR\WinRAR.exe" a  -r -ep1 -x*\*DoNotShip* -x*\*DontShip* -x*\*DoNotShip*\ -x*\*DontShip*\ ".\%gameName% - %1 - %ver% - release.zip" ".\%1\*"
if errorlevel 1 set packFailed=1
exit /b
