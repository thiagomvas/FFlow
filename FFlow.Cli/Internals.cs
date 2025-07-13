namespace FFlow.Cli;

public static class Internals
{
    public static Dictionary<string, string> PackageMappings = new()
    {
        { ".NET SDK", "FFlow.Steps.DotNet" },
        { "Shell Commands", "FFlow.Steps.Shell" },
    };
    
}