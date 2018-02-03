@echo off

if "%1"=="clean" goto clean

set compile-tool="C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
%compile-tool% /target:winexe /out:encrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartEncryptForm.cs
%compile-tool% /target:winexe /out:decrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartDecryptForm.cs
%compile-tool%  /out:kanban-form.exe /win32icon:board-icon.ico Kanban\KanbanMainForm.cs
goto end

:clean
del *.exe
del *.dat
goto end

:end