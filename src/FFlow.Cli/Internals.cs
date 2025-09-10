namespace FFlow.Cli;

public static class Internals
{
    public static Dictionary<string, string> PackageMappings = new()
    {
        { ".NET SDK", "FFlow.Steps.DotNet" },
        { "Shell Commands", "FFlow.Steps.Shell" },
        { "SFTP", "FFlow.Steps.SFTP" },
        { "File IO", "FFlow.Steps.FileIO" },
        { "HTTP Requests", "FFlow.Steps.Http" },
        { "Scheduling", "FFlow.Scheduling" },
        { "Metrics & Observability", "FFlow.Observability" },
    };
    
    public const string DockerImage = "mcr.microsoft.com/dotnet/sdk:10.0-preview";
}