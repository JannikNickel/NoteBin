@echo off

:: Build frontend
call npm run --prefix "%~dp0/frontend" build

if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

:: Build backend
dotnet build

if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

:: Run backend
@REM dotnet run --project "%~dp0src\NoteBin.csproj" --launch-profile https
pushd "./src/bin/Debug/net9.0/"
set ASPNETCORE_ENVIRONMENT=Development
"NoteBin.exe" --urls "https://localhost:7188;http://localhost:5298"
popd
