@echo off

if "%1"=="clean" goto clean

set compile-tool="C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe"
%compile-tool% /out:encrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartEncryptForm.cs
%compile-tool% /out:decrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartDecryptForm.cs
goto end

:clean
del *.exe
del *.dat
goto end

:end