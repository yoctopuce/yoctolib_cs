@ECHO OFF
if "%VCINSTALLDIR%"=="" call "%VS100COMNTOOLS%vsvars32.bat"
echo.
echo Build yapi static and dynamic libray for C#
echo ============================================


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