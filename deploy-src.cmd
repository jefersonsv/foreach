SET app=ForeachUtility
rd /s /q .\src\%app%\bin\Release
dotnet build -c Release .\src\%app%\%app%.csproj
nuget push .\src\%app%\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json