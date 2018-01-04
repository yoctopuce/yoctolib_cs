@ECHO OFF
if "%VCINSTALLDIR%"=="" call "%VS100COMNTOOLS%vsvars32.bat"
echo.

IF "%1" == "" goto build
IF %1 == test_release goto test_release
IF %1 == release goto release
IF %1 == clean goto clean
echo INVALID TARGET
goto end

:build
msbuild
goto end

:clean
msbuild /t:Clean
del /Q /F bin\Debug\*
rmdir /S /Q obj
goto end

:release
msbuild
IF ERRORLEVEL 1 goto error
del /Q /F bin\Debug\*.vshost* bin\Debug\*.pdb
rmdir /S /Q obj
goto end

:test_release
msbuild
IF ERRORLEVEL 1 goto error
msbuild /t:Clean
del /Q /F bin\Debug\*
rmdir /S /Q obj
goto end


:error
echo msbuild failled
exit /b 1
:end