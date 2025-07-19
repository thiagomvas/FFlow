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

        var scriptPath = Path.GetFullPath(positionalArgs[0]);
        if (!File.Exists(scriptPath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Script file not found: {scriptPath}");
            return 1;
        }

        string? rootDir = null;
        var hasRoot = options.TryGetValue("root", out var rootOption);
        if (hasRoot)
        {
            rootDir = Path.GetFullPath(rootOption);
            if (!Directory.Exists(rootDir))
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Root directory does not exist: {rootDir}");
                return 1;
            }
        }

        var guid = Guid.NewGuid().ToString("N");
        var tempFolder = $"/tmp/.{guid}";
        var containerScriptPath = hasRoot && IsSubPathOf(scriptPath, rootDir)
            ? $"{tempFolder}/{Path.GetRelativePath(rootDir!, scriptPath).Replace("\\", "/")}"
            : $"{tempFolder}/{Path.GetFileName(scriptPath)}";

        var dockerArgs = new List<string>
        {
            "run", "--rm"
        };

        if (hasRoot)
            dockerArgs.Add($"-v \"{rootDir}:/mnt/root:rw\"");

        dockerArgs.Add($"-v \"{scriptPath}:/mnt/script/{Path.GetFileName(scriptPath)}:ro\"");

        if (options.TryGetValue("include", out var includeVal))
        {
            var includes =
                includeVal.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var include in includes)
            {
                var fullInclude = Path.GetFullPath(include);
                if (!File.Exists(fullInclude) && !Directory.Exists(fullInclude))
                {
                    AnsiConsole.MarkupLine($"[yellow]Warning:[/] Include path not found: {include}");
                    continue;
                }

                if (hasRoot && IsSubPathOf(fullInclude, rootDir!))
                    continue;

                var target = $"/mnt/include/{Path.GetFileName(fullInclude)}";
                dockerArgs.Add($"-v \"{fullInclude}:{target}:ro\"");
            }
        }

        dockerArgs.Add("-w /mnt");

        dockerArgs.Add(Internals.DockerImage);

        // Copy root + script into isolated /tmp folder, then run it
        var runScript = $"""
                             mkdir -p "{tempFolder}";
                             {(hasRoot ? $"cp -r /mnt/root/* \"{tempFolder}/\";" : "")}
                             cp "/mnt/script/{Path.GetFileName(scriptPath)}" "{containerScriptPath}";
                             cd root
                             dotnet run --verbosity quiet "{containerScriptPath}"
                         """;

        dockerArgs.Add("sh");
        dockerArgs.Add("-c");
        dockerArgs.Add($"\"{runScript}\"");

        var dockerArgsStr = string.Join(" ", dockerArgs);

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
                    Arguments = dockerArgsStr,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using var process = new Process { StartInfo = psi };

                void OnOutput(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        if (!startedExecution)
                        {
                            ctx.Status("Running pipeline...");
                            ctx.Spinner(Spinner.Known.Line);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            startedExecution = true;
                        }

                        var safe = e.Data.Replace("[", "[[").Replace("]", "]]");
                        AnsiConsole.MarkupLine(safe);
                    }
                }

                void OnError(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        if (!startedExecution)
                        {
                            ctx.Status("Running pipeline...");
                            ctx.Spinner(Spinner.Known.Line);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            startedExecution = true;
                        }

                        var safe = e.Data.Replace("[", "[[").Replace("]", "]]");
                        AnsiConsole.MarkupLine($"[red]Error:[/] {safe}");
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
                    ctx.Status("No output from container.");
                    ctx.Spinner(Spinner.Known.Arrow);
                    ctx.SpinnerStyle(Style.Parse("grey"));
                }

                exitCode = process.ExitCode;
            });

        return exitCode;
    }


    private static bool IsSubPathOf(string path, string baseDir)
    {
        var normalizedPath =
            Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) +
            Path.DirectorySeparatorChar;
        var normalizedBase =
            Path.GetFullPath(baseDir).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) +
            Path.DirectorySeparatorChar;
        return normalizedPath.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase);
    }

    public void DisplayHelp()
    {
        AnsiConsole.MarkupLine("[bold]Usage:[/] fflow run <file.cs> [--include=path1,path2,...] [--root=dir]");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("Runs the specified C# file inside the official .NET 10 SDK Docker container.");
        AnsiConsole.MarkupLine("Mounts --root directory as /app if specified.");
        AnsiConsole.MarkupLine("If script is outside --root, mounts script folder separately.");
        AnsiConsole.MarkupLine("Includes are mounted inside /app unless inside root or script folder.");
    }
}