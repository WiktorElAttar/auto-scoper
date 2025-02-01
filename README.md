# auto-scoper

## NuGet Integration Tests
- `dotnet pack -c Release -o ./artifacts -p:Version=0.0.1-beta`
- `dotnet restore ./tests/AutoScoper.Tests.NugetIntegration --packages ./packages --configfile "nuget.integration-tests.config"`
- `dotnet build ./tests/AutoScoper.Tests.NugetIntegration -c Release --packages ./packages --no-restore`
- `dotnet test ./tests/AutoScoper.Tests.NugetIntegration -c Release --no-build --no-restore`