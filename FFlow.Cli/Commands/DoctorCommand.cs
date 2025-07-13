using Spectre.Console;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FFlow.Cli.Commands;

public partial class DoctorCommand : ICommand
{
    public string Name => "doctor";
    public string Description => "Run diagnostics on the FFlow CLI environment.";

    public List<ICommand> Subcommands { get; } = new();

    public void Configure(CommandBuilder builder)
    {
    }

    public int Execute(List<string> positionalArgs, Dictionary<string, string> options)
    {
        var results = new List<CheckResult>();

        AnsiConsole.Status()
            .Spinner(Spectre.Console.Spinner.Known.Dots)
            .Start("Checking system dependencies...", ctx =>
            {
                results.Add(CheckDotnetSdk());
                results.Add(CheckDocker());
                results.Add(CheckDockerImage());
            });

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Component[/]")
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Details[/]");

        foreach (var result in results)
        {
            table.AddRow(
                $"[white]{result.Name}[/]",
                ToColoredStatus(result.Status),
                result.Details
            );
        }
        AnsiConsole.Write(table);
        AnsiConsole.Markup("\n[grey]NOTE: If the image isn't available locally, it will be installed when running a pipeline.[/]\n");

        return 0;
    }

    public void DisplayHelp()
    {
        var options = new Dictionary<string, (string Description, string? ShortName)>
        {
            { "help", ("Show this help message.", "h") }
        };

        HelpHelper.ShowHelp(Name, Description, null,
            options.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Description));
    }


    private static CheckResult CheckDocker()
    {
        var processVersion = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        processVersion.Start();
        string outputVersion = processVersion.StandardOutput.ReadToEnd();
        processVersion.WaitForExit();

        if (string.IsNullOrWhiteSpace(outputVersion))
        {
            return new CheckResult("Docker", "Missing", "FFlow requires Docker to run.");
        }

        var versionPattern = VersionPattern();
        var match = versionPattern.Match(outputVersion);

        if (!match.Success ||
            !int.TryParse(match.Groups[1].Value, out int major) ||
            !int.TryParse(match.Groups[2].Value, out int minor))
        {
            return new CheckResult("Docker", "Missing", "Unable to determine Docker version.");
        }

        string versionStatus;
        string versionDetails;

        if (major >= 20)
        {
            versionStatus = "OK";
            versionDetails = $"Found Docker version {major}.{minor}";
        }
        else
        {
            versionStatus = "Warning";
            versionDetails = "FFlow requires Docker 20.0 or higher for optimal performance.";
        }

        return new CheckResult("Docker", versionStatus, $"{versionDetails}.");
    }
    
    private static CheckResult CheckDockerImage()
    {
        var dockerImage = Internals.DockerImage;

        var processImage = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"images -q {dockerImage}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        processImage.Start();
        string imageId = processImage.StandardOutput.ReadToEnd().Trim();
        processImage.WaitForExit();

        if (string.IsNullOrEmpty(imageId))
        {
            return new CheckResult("Docker Image", "Warning", $"Image '{dockerImage}' is not installed locally.");
        }

        return new CheckResult("Docker Image", "OK", $"Image '{dockerImage}' is present.");
    }


    private static CheckResult CheckDotnetSdk()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-sdks",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        if (string.IsNullOrWhiteSpace(output))
        {
            return new CheckResult(".NET SDK", "Missing", "FFlow requires .NET SDK 10.0 or higher.");
        }

        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var versionPattern = VersionPattern();

        bool foundDotnet9 = false;

        foreach (var line in lines)
        {
            var match = versionPattern.Match(line.Trim());
            if (match.Success &&
                int.TryParse(match.Groups[1].Value, out int major) &&
                int.TryParse(match.Groups[2].Value, out int minor))
            {
                if (major > 10 || (major == 10 && minor >= 0))
                {
                    return new CheckResult(".NET SDK", "OK", $"Found SDK version {major}.{minor}");
                }
                else if (major == 9)
                {
                    foundDotnet9 = true;
                }
            }
        }

        if (foundDotnet9)
        {
            return new CheckResult(".NET SDK", "Warning",
                "FFlow supports only project-based workflows on .NET SDK 9.x.");
        }

        return new CheckResult(".NET SDK", "Missing", "FFlow requires .NET SDK 10.0 or higher.");
    }


    private static string ToColoredStatus(string status)
    {
        return status switch
        {
            "OK" => $"[green]{Emoji.Known.CheckMark} OK[/]",
            "Warning" => $"[yellow]{Emoji.Known.Warning}️ Warning[/]",
            "Missing" => $"[red]{Emoji.Known.CrossMark} Missing[/]",
            _ => $"[grey]{status}[/]"
        };
    }

    private record CheckResult(string Name, string Status, string Details);

    [GeneratedRegex(@"(\d+)\.(\d+)\.\d+")]
    private static partial Regex VersionPattern();
}