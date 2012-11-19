@echo off
set FRAMEWORK_PATH=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%FRAMEWORK_PATH%;

:target_config
set TARGET_CONFIG=Debug
IF x==%1x goto framework_version
set TARGET_CONFIG=%1

:framework_version
set FRAMEWORK_VERSION=v4.0
set ILMERGE_VERSION=v4,%FRAMEWORK_PATH%
if x==%2x goto build
set FRAMEWORK_VERSION=%2
set ILMERGE_VERSION=%3

:build
if exist output ( rmdir /s /q output )
if exist publish ( rmdir /s /q publish )

mkdir output
mkdir output\bin
mkdir output\nuget

echo === COMPILING ===
echo Compiling / Target: %FRAMEWORK_VERSION% / Config: %TARGET_CONFIG%
msbuild /nologo /verbosity:quiet aec.cqrs.sln /p:Configuration=%TARGET_CONFIG% /t:Clean
msbuild /nologo /verbosity:quiet aec.cqrs.sln /p:Configuration=%TARGET_CONFIG% /p:TargetFrameworkVersion=%FRAMEWORK_VERSION%

echo.
echo === TESTS ===
echo Unit Tests
echo Skipping Unit Tests... Update the build script to include tests.

echo.
echo === MERGING ===
echo Merging Primary Assembly
set FILES_TO_MERGE=
set FILES_TO_MERGE=%FILES_TO_MERGE% "src\app\Aec.Cqrs\bin\%TARGET_CONFIG%\Aec.Cqrs.dll"
(echo.|set /p =Aec.*)>exclude.txt
thirdparty\tools\ILMerge\ILMerge.exe /keyfile:Aec.snk /internalize:"exclude.txt" /xmldocs /wildcards /targetplatform:%ILMERGE_VERSION% /out:output/bin/Aec.dll %FILES_TO_MERGE%
del exclude.txt

echo.
echo === COPYING DLLs ===
copy src\app\Aec.Infrastructure\bin\%TARGET_CONFIG%\Aec.Infrastructure.* output\bin
copy src\app\Aec.Cqrs\bin\%TARGET_CONFIG%\Aec.Cqrs.* output\bin
copy src\app\Aec.Cqrs.Client\bin\%TARGET_CONFIG%\Aec.Cqrs.Client* output\bin
copy src\app\Aec.Cqrs.EventStorage\bin\%TARGET_CONFIG%\Aec.Cqrs.EventStorage* output\bin

echo Rereferencing Merged Assembly
msbuild /nologo /verbosity:quiet aec.cqrs.sln /p:Configuration=%TARGET_CONFIG% /t:Clean
msbuild /nologo /verbosity:quiet aec.cqrs.sln /p:Configuration=%TARGET_CONFIG% /p:ILMerged=true /p:TargetFrameworkVersion=%FRAMEWORK_VERSION%

echo.
echo === CREATING NUGET PACKAGE ===
set NUGET_REPO_PATH=C:\Users\asahin\Documents\AecNuGetRepository

echo Creating Aec.Infrastructure package
copy src\packaging\Nuget\Aec.Infrastructure.nuspec output\bin
thirdparty\tools\Nuget\nuget.exe pack output\bin\Aec.Infrastructure.nuspec
copy *.nupkg %NUGET_REPO_PATH%
move %NUGET_REPO_PATH%\Aec.Infrastructure.1.*.nupkg %NUGET_REPO_PATH%\Aec.Infrastructure.nupkg
copy *.nupkg output\nuget
del *.nupkg
del output\bin\Aec.Infrastructure.nuspec

echo Creating Aec.Cqrs package
copy src\packaging\Nuget\Aec.Cqrs.nuspec output\bin
thirdparty\tools\Nuget\nuget.exe pack output\bin\Aec.Cqrs.nuspec
copy *.nupkg %NUGET_REPO_PATH%
move %NUGET_REPO_PATH%\Aec.Cqrs.1.*.nupkg %NUGET_REPO_PATH%\Aec.Cqrs.nupkg
copy *.nupkg output\nuget
del *.nupkg
del output\bin\Aec.Cqrs.nuspec

echo Creating Aec.Cqrs.Client package
copy src\packaging\Nuget\Aec.Cqrs.Client.nuspec output\bin
thirdparty\tools\Nuget\nuget.exe pack output\bin\Aec.Cqrs.Client.nuspec
copy *.nupkg %NUGET_REPO_PATH%
move %NUGET_REPO_PATH%\Aec.Cqrs.Client.1.*.nupkg %NUGET_REPO_PATH%\Aec.Cqrs.Client.nupkg
copy *.nupkg output\nuget
del *.nupkg
del output\bin\Aec.Cqrs.Client.nuspec

echo Creating Aec.Cqrs.EventStorage package
copy src\packaging\Nuget\Aec.Cqrs.EventStorage.nuspec output\bin
thirdparty\tools\Nuget\nuget.exe pack output\bin\Aec.Cqrs.EventStorage.nuspec
copy *.nupkg %NUGET_REPO_PATH%
move %NUGET_REPO_PATH%\Aec.Cqrs.EventStorage.1.*.nupkg %NUGET_REPO_PATH%\Aec.Cqrs.EventStorage.nupkg
copy *.nupkg output\nuget
del *.nupkg
del output\bin\Aec.Cqrs.EventStorage.nuspec

move output publish

echo.
echo === CLEANUP ===
echo Cleaning Build
msbuild /nologo /verbosity:quiet aec.cqrs.sln /p:Configuration=%TARGET_CONFIG% /t:Clean

echo.
echo === DONE ===