namespace FFlow.Cli.Commands;

public class RootCommand : ICommand
{
    public string Name => "";
    public string Description => "Root command";

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
}
