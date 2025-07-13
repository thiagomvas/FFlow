using FFlow.Cli;
using FFlow.Cli.Commands;
using Spectre.Console;

var root = new RootCommand();

var doctor = new DoctorCommand();
var init = new InitCommand();
var log = new LogCommand();

root.Subcommands.Add(doctor);
root.Subcommands.Add(init);
root.Subcommands.Add(log);

var dispatcher = new CommandDispatcher(root);

return dispatcher.Dispatch(args);

class LogCommand : ICommand
{
    public string Name => "log";
    public string Description => "Logs the received positional arguments and options for debugging.";
    public List<ICommand> Subcommands => new();

    public void Configure(CommandBuilder builder)
    {
        // No options needed for now
    }

    public int Execute(List<string> positionalArgs, Dictionary<string, string> options)
    {
        AnsiConsole.MarkupLine("[bold underline green]FFlow Argument Logger[/]");
        AnsiConsole.WriteLine();

        // Print positional arguments
        AnsiConsole.MarkupLine("[bold]Positional Arguments:[/]");
        if (positionalArgs.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey](none)[/]");
        }
        else
        {
            for (int i = 0; i < positionalArgs.Count; i++)
                AnsiConsole.MarkupLine($"  [[{i}]]: {positionalArgs[i]}");
        }

        AnsiConsole.WriteLine();

        // Print options
        AnsiConsole.MarkupLine("[bold]Options:[/]");
        if (options.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey](none)[/]");
        }
        else
        {
            foreach (var (key, value) in options)
                AnsiConsole.MarkupLine($"  [green]{key}[/] = {value}");
        }

        return 0;
    }

    public void DisplayHelp()
    {
        AnsiConsole.MarkupLine("[bold]Usage:[/] fflow log [args] [--options]");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("Logs the received positional arguments and options for debugging.");
    }
}
