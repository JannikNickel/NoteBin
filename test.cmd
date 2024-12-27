:: Build frontend
call npm run --prefix "%~dp0frontend" build

:: Run backend
set ASPNETCORE_ENVIRONMENT=Development
dotnet run --project "%~dp0src\NoteBin.csproj" --launch-profile https
