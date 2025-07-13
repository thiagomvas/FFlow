namespace FFlow.Cli;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    List<ICommand> Subcommands { get; }
    void Configure(CommandBuilder builder);
    int Execute(List<string> positionalArgs, Dictionary<string, string> options);
}
