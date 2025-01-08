@echo off

:: Delete publish folder
rmdir /S /Q "%~dp0publish" > nul 2>&1

:: Install npm packages
pushd "%~dp0/frontend"
call npm install
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
popd

:: Build frontend
call npm run --prefix "%~dp0/frontend" build
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

:: Build backend
dotnet build "%~dp0src/NoteBin.csproj" -c Release -o "publish" -p:PublishProfile=Publish
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

:: Copy web to publish folder
xcopy "%~dp0/web" "%~dp0/publish/web" /E /I /Y /Q
