using Spectre.Console;

namespace FFlow.Cli.Commands;

public class RootCommand : ICommand
{
    public string Name => "";
    public string Description => "CLI tooling for FFlow — a lightweight, fluent .NET workflow automation framework built for CI/CD, DevOps, and backend orchestration.";

    public List<ICommand> Subcommands { get; } = new();

    public void Configure(CommandBuilder builder)
    {
        // Root has no options by default
    }

    public int Execute(List<string> positionalArgs, Dictionary<string, string> options)
    {
        Console.WriteLine("Please specify a command.");
        return 1;
    }

    public void DisplayHelp()
    {
        var options = new Dictionary<string, string>
        {
            { "help, h", "Show help information." }
        };

        var commands = new Dictionary<string, string>
        {
            { "doctor", "Run diagnostics on the FFlow CLI environment." },
            { "help", "Show help information." }
        };
        
        HelpHelper.ShowHelp(Name, Description, commands, options);
    }


}
