@echo off
echo Building System.Abstract:
PowerShell -Command ".\psake.ps1"

rm %LocalAppData%\NuGet\Cache\System.Abstract.1.0.0.nupkg
If Not "%NugetPackagesDir%" == "" xcopy .\_build\*.nupkg %NugetPackagesDir% /Y/Q
If Not "%NugetPackagesDir%" == "" del %NugetPackagesDir%\*.symbols.nupkg /Q
