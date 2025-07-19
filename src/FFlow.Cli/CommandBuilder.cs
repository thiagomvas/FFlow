namespace FFlow.Cli;

public class CommandBuilder
{
    private readonly List<CommandOption> _options = new();
    
    public void AddOption(string name, string description, string shortName = "")
    {
        _options.Add(new CommandOption(name, description, shortName));
    }
    
    private readonly record struct CommandOption(string Name, string Description, string ShortName = "");
}
