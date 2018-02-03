@echo off

if "%1"=="clean" goto clean

:: we always use the newest compiler to compile the project
set compile-tool=
for /f %%i in ('dir C:\Windows\Microsoft.NET\Framework\csc.exe /b /s') do (
    set compile-tool=%%i
)
if "%compile-tool%"=="" goto end
%compile-tool% /target:winexe /out:encrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartEncryptForm.cs
%compile-tool% /target:winexe /out:decrypt-form.exe /win32icon:encrypt-icon.ico DecryptTool\Utils.cs DecryptTool\EncryptTool.cs DecryptTool\DecryptTool.cs DecryptTool\CryptologyForm.cs DecryptTool\StartDecryptForm.cs
%compile-tool%  /out:kanban-form.exe /win32icon:board-icon.ico Kanban\KanbanMainForm.cs
goto end

:clean
del *.exe
del *.dat
goto end

:end