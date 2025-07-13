using Spectre.Console;
using System.Diagnostics;

namespace FFlow.Cli.Commands;

public class RunCommand : ICommand
{
    public string Name => "run";
    public string Description => "Runs a C# file inside the official .NET 10 SDK Docker container.";
    public List<ICommand> Subcommands => new();

    public void Configure(CommandBuilder builder)
    {
        // Add options here if needed
    }

    public int Execute(List<string> positionalArgs, Dictionary<string, string> options)
    {
        if (positionalArgs.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] No C# file specified.");
            return 1;
        }

        var filePath = positionalArgs[0];

        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {filePath}");
            return 1;
        }

        var absPath = Path.GetFullPath(filePath);
        var workDir = Path.GetDirectoryName(absPath)!;
        var fileName = Path.GetFileName(absPath);

        AnsiConsole.MarkupLine($"[grey]Starting container and running [italic]{fileName}[/]...[/]");

        var dockerImage = "mcr.microsoft.com/dotnet/sdk:10.0-preview";

        var dockerArgs = $"run --rm -v \"{workDir}:/app\" -w /app {dockerImage} dotnet run {fileName}";

        var psi = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = dockerArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            using var process = Process.Start(psi);
            string stdout = process!.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(stdout))
                AnsiConsole.WriteLine(stdout);

            if (!string.IsNullOrWhiteSpace(stderr))
                AnsiConsole.MarkupLine($"[red]Error:[/] {stderr}");

            return process.ExitCode;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to run Docker container:[/] {ex.Message}");
            return 1;
        }
    }

    public void DisplayHelp()
    {
        AnsiConsole.MarkupLine("[bold]Usage:[/] fflow run <file.cs>");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("Runs the specified C# file inside the official .NET 10 SDK Docker container.");
        AnsiConsole.MarkupLine("The folder containing the file is mounted inside the container.");
        AnsiConsole.MarkupLine("The container is removed automatically after execution.");
    }
}
