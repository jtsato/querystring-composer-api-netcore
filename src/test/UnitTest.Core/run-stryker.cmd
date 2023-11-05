@echo off
cls
echo.

rd /s /q bin
rd /s /q obj

dotnet restore --force --no-cache
dotnet clean
dotnet build --configuration Debug --no-restore

dotnet tool restore
dotnet stryker -o