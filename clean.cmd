@echo off
cls
echo.

dotnet clean

for /f %%i in ('dir bin /s /b') do rd /s /q %%i
for /f %%i in ('dir obj /s /b') do rd /s /q %%i
for /f %%i in ('dir StrykerOutput /s /b') do rd /s /q %%i
