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
        AnsiConsole.Write(new Rule().RuleStyle("grey"));

        return 0;
    }

    public void DisplayHelp()
    {
        var options = new Dictionary<string, (string Description, string? ShortName)>
        {
            { "check-docker", ("Check Docker installation.", null) },
            { "check-dotnet", ("Check .NET 10 SDK installation.", null) },
            { "help", ("Show this help message.", "h") }
        };
        
        HelpHelper.ShowHelp(Name, Description, null, options.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Description));
    }


    private static CheckResult CheckDocker()
    {
        var process = new Process
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

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        if (string.IsNullOrWhiteSpace(output))
        {
            return new CheckResult("Docker", "Missing", "FFlow requires Docker to run.");
        }

        var versionPattern = VersionPattern();
        var match = versionPattern.Match(output);

        if (match.Success &&
            int.TryParse(match.Groups[1].Value, out int major) &&
            int.TryParse(match.Groups[2].Value, out int minor))
        {
            if (major >= 20)
            {
                return new CheckResult("Docker", "OK", $"Found Docker version {major}.{minor}");
            }
            else
            {
                return new CheckResult("Docker", "Warning", "FFlow requires Docker 20.0 or higher for optimal performance.");
            }
        }

        return new CheckResult("Docker", "Missing", "FFlow requires Docker to run.");
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
                    return new CheckResult("Dotnet SDK", "OK", $"Found SDK version {major}.{minor}");
                }
                else if (major == 9)
                {
                    foundDotnet9 = true;
                }
            }
        }

        if (foundDotnet9)
        {
            return new CheckResult("Dotnet SDK", "Warning", "FFlow supports only project-based workflows on .NET SDK 9.x.");
        }

        return new CheckResult("Dotnet SDK", "Missing", "FFlow requires .NET SDK 10.0 or higher.");
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

