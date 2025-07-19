namespace FFlow.Cli;

public static class Internals
{
    public static Dictionary<string, string> PackageMappings = new()
    {
        { ".NET SDK", "FFlow.Steps.DotNet" },
        { "Shell Commands", "FFlow.Steps.Shell" },
    };
    
    public const string DockerImage = "mcr.microsoft.com/dotnet/sdk:10.0-preview";
}