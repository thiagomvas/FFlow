namespace FFlow.Cli;

public class CommandDispatcher
{
    private readonly ICommand _rootCommand;

    public CommandDispatcher(ICommand rootCommand)
    {
        _rootCommand = rootCommand;
    }

    public int Dispatch(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No command specified.");
            return 1;
        }

        var currentCommand = _rootCommand;
        int index = 0;

        // Traverse subcommands based on args
        while (index < args.Length && currentCommand.Subcommands.Any(c => c.Name == args[index].ToLowerInvariant()))
        {
            currentCommand = currentCommand.Subcommands.First(c => c.Name == args[index].ToLowerInvariant());
            index++;
        }

        // Parse options and positional args for the currentCommand
        var positionalArgs = new List<string>();
        var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (; index < args.Length; index++)
        {
            var arg = args[index];
            if (arg.StartsWith("--"))
            {
                var opt = arg[2..];
                string? val = null;

                if (opt.Contains('='))
                {
                    var split = opt.Split('=', 2);
                    opt = split[0];
                    val = split[1];
                }
                else if (index + 1 < args.Length && !args[index + 1].StartsWith("--"))
                {
                    val = args[++index];
                }
                else
                {
                    val = "true"; // flag option
                }

                options[opt] = val!;
            }
            else
            {
                positionalArgs.Add(arg);
            }
        }

        return currentCommand.Execute(positionalArgs, options);
    }
}
