@ECHO OFF
if "%VCINSTALLDIR%"=="" call "%VS140COMNTOOLS%vsvars32.bat"
if "%VCINSTALLDIR%"=="" call "%VS100COMNTOOLS%vsvars32.bat"
if "%VCINSTALLDIR%"=="" call "%VS110COMNTOOLS%vsvars32.bat"
if "%VCINSTALLDIR%"=="" call "%VS120COMNTOOLS%vsvars32.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Entreprise\Common7\Tools\VsDevCmd.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\Common7\Tools\VsDevCmd.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Entreprise\Common7\Tools\VsDevCmd.bat"
if "%VCINSTALLDIR%"=="" call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
echo.
echo Build yapi static and dynamic libray for C# 2.0
echo ===============================================


set failled=
FOR /D %%A IN (Examples\*) DO (call:BuildDir %%A %1)
IF NOT DEFINED failled goto end

echo.
echo COMPILATION HAS FAILLED ON DIRECTORIES :
echo %failled%

goto error
:BuildDir
echo build %~1 %~2
cd %~1
call build.bat %~2
IF ERRORLEVEL 1 set failled=%failled% %~1
cd ..\..\
echo done
GOTO:EOF

goto end
:error
echo error
exit /b 1
:end