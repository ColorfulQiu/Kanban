@echo off

if "%1"=="clean" goto clean

set compile-tool="C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe"
%compile-tool% /out:encrypt.exe DecryptTool\EncryptTool.cs
goto end

:clean
del *.exe
del *.dat
goto end

:end