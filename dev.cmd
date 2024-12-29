:: Build frontend
call npm run --prefix "%~dp0frontend" build

if %ERRORLEVEL% NEQ 0 (
    exit /b %ERRORLEVEL%
)

:: Run backend
set ASPNETCORE_ENVIRONMENT=Development
dotnet run --project "%~dp0src\NoteBin.csproj" --launch-profile https
