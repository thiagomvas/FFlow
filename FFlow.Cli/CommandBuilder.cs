namespace FFlow.Cli;

public class CommandBuilder
{
    public Dictionary<string, string> OptionDescriptions { get; } = new();

    public void AddOption(string name, string description = "")
    {
        OptionDescriptions[name] = description;
    }
}
