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
        var dockerImage = Internals.DockerImage;
        var dockerArgs = $"run --rm -v \"{workDir}:/app\" -w /app {dockerImage} dotnet run {fileName}";

        int exitCode = 1;
        bool startedExecution = false;

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("yellow"))
            .Start("Setting up Docker container...", ctx =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = dockerArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using var process = new Process() { StartInfo = psi };

                void OnOutput(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        if (!startedExecution)
                        {
                            ctx.Status("Running pipeline...");
                            ctx.Spinner(Spinner.Known.Line);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            startedExecution = true;
                        }

                        var safeLine = e.Data.Replace("[", "[[").Replace("]", "]]");
                        AnsiConsole.MarkupLine(safeLine);
                    }
                }

                void OnError(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        if (!startedExecution)
                        {
                            ctx.Status("Running pipeline...");
                            ctx.Spinner(Spinner.Known.Line);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            startedExecution = true;
                        }

                        var safeLine = e.Data.Replace("[", "[[").Replace("]", "]]");
                        AnsiConsole.MarkupLine($"[red]Error:[/] {safeLine}");
                    }
                }

                process.OutputDataReceived += OnOutput;
                process.ErrorDataReceived += OnError;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (!startedExecution)
                {
                    // No output at all, mark status as done
                    ctx.Status("No output from container.");
                    ctx.Spinner(Spinner.Known.Arrow);
                    ctx.SpinnerStyle(Style.Parse("grey"));
                }

                exitCode = process.ExitCode;
            });

        return exitCode;
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