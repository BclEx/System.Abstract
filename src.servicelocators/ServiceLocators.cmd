@echo off
echo Building ServiceLocators:
PowerShell -Command ".\psake.ps1"

If Not "%NugetPackagesDir%" == "" xcopy .\_build\*.nupkg %NugetPackagesDir% /Y/Q
If Not "%NugetPackagesDir%" == "" del %NugetPackagesDir%\*.symbols.nupkg /Q
